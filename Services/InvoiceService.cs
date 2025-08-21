
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;
using FixoraBackend.Models;
using Microsoft.EntityFrameworkCore;
using FixoraBackend.Services;

namespace FixoraBackend.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _context;
        private readonly IReferralService _referralService;

        public InvoiceService(AppDbContext context, IReferralService referralService)
        {
            _context = context;
            _referralService = referralService;
        }

        public async Task<Invoice> CreateInvoiceAsync(CreateInvoiceRequest request, string createdByUserId)
        {
            var invoice = new Invoice
            {
                InvoiceNumber = $"INV-{DateTime.UtcNow.Ticks}",
                ClientId = request.ClientId,
                BusinessClientId = request.BusinessClientId,
                BusinessSiteId = request.BusinessSiteId,
                OrderId = request.OrderId,
                TotalAmount = request.TotalAmount,
                FirstPaymentAmount = request.FirstPaymentAmount,
                RequiresFirstPayment = request.RequiresFirstPayment,
                WarrantyMonths = request.WarrantyMonths,
                InstallmentsCount = request.InstallmentsCount,
                AppCommissionRate = request.AppCommissionRate,
                ExpiryDate = request.ExpiryDate,
                ProviderNote = request.ProviderNote,
                Status = "Pending",
                CreatedByUserId = createdByUserId,
                CreatedAt = DateTime.UtcNow,
                Items = request.Items.Select(i => new InvoiceItem
                {
                    Title = i.Title,
                    Description = i.Description,
                    Amount = i.Amount
                }).ToList(),
                StatusHistory = new List<InvoiceStatusHistory>
                {
                    new InvoiceStatusHistory
                    {
                        Status = "Pending",
                        ChangedByUserId = createdByUserId,
                        Note = "Initial invoice created",
                        ChangedAt = DateTime.UtcNow
                    }
                }
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<bool> ApproveOrRejectInvoiceAsync(ApproveInvoiceRequest request, string customerId)
        {
            var invoice = await _context.Invoices
                .Include(i => i.StatusHistory)
                .FirstOrDefaultAsync(i => i.Id == request.InvoiceId && i.ClientId == customerId);

            if (invoice == null || invoice.Status != "Pending")
                return false;

            invoice.CustomerNote = request.CustomerNote;

            if (request.Approve)
            {
                invoice.Status = invoice.RequiresFirstPayment ? "AwaitingFirstPayment" : "Approved";
                invoice.StatusHistory.Add(new InvoiceStatusHistory
                {
                    Status = invoice.Status,
                    ChangedByUserId = customerId,
                    Note = "Approved by client",
                    ChangedAt = DateTime.UtcNow
                });
            }
            else
            {
                invoice.Status = "Rejected";
                invoice.StatusHistory.Add(new InvoiceStatusHistory
                {
                    Status = "Rejected",
                    ChangedByUserId = customerId,
                    Note = $"Rejected - {request.CustomerNote}",
                    ChangedAt = DateTime.UtcNow
                });
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> MarkInvoiceAsPaidAsync(int invoiceId)
        {
            var invoice = await _context.Invoices.FindAsync(invoiceId);
            if (invoice == null) return false;

            invoice.IsPaid = true;
            invoice.PaidAt = DateTime.UtcNow;

            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();

            // ✅ إضافة العمولة إذا كان العميل تمت إحالته
            var referral = await _context.Referrals
                .FirstOrDefaultAsync(r => r.ReferredUserId == invoice.ClientUserId && !r.IsCommissionPaid);

            if (referral != null)
            {
                decimal commissionAmount = invoice.Amount * 0.05m; // 5% عمولة الإحالة
                await _referralService.AddCommissionAsync(referral.Id, commissionAmount);
            }

            return true;
        }


        public async Task<List<Invoice>> GetInvoicesByOrderAsync(string orderId)
        {
            return await _context.Invoices
                .Include(i => i.Installments)
                .Where(i => i.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<List<Invoice>> GetInvoicesByUserAsync(string userId)
        {
            return await _context.Invoices
                .Include(i => i.Installments)
                .Where(i => i.ClientId == userId || i.ProviderId == userId)
                .ToListAsync();
        }




        public async Task<Invoice> CreateInvoiceAsync(string providerId, CreateInvoiceRequest request, string clientId)
        {
            // التحقق من أن مجموع الدفعات = المبلغ الإجمالي
            var totalInstallments = request.Installments.Sum(i => i.Amount);
            if (totalInstallments != request.TotalAmount)
                throw new InvalidOperationException("مجموع الدفعات لا يساوي المبلغ الإجمالي للفاتورة");

            var invoice = new Invoice
            {
                OrderId = request.OrderId,
                ProviderId = providerId,
                ClientId = clientId,
                TotalAmount = request.TotalAmount,
                Description = request.Description,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            // إضافة الدفعات
            foreach (var inst in request.Installments)
            {
                _context.InvoiceInstallments.Add(new InvoiceInstallment
                {
                    InvoiceId = invoice.Id,
                    Amount = inst.Amount,
                    DueDate = inst.DueDate ?? DateTime.UtcNow,
                    IsPaid = false
                });
            }

            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<bool> UpdateInvoiceStatusAsync(int invoiceId, string status)
        {
            var invoice = await _context.Invoices.FindAsync(invoiceId);
            if (invoice == null) return false;

            invoice.Status = status;
            _context.Invoices.Update(invoice);
            return await _context.SaveChangesAsync() > 0;
        }




        public async Task<bool> ApproveAndPayInvoiceAsync(int invoiceId, string clientId)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Installments)
                .FirstOrDefaultAsync(i => i.Id == invoiceId && i.ClientId == clientId);

            if (invoice == null) return false;

            // تحديث الحالة إلى موافق عليها
            invoice.Status = "Approved";

            if (invoice.DownPayment > 0)
            {
                // محاولة الدفع من المحفظة
                var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == clientId);
                if (wallet == null || wallet.Balance < invoice.DownPayment)
                    throw new InvalidOperationException("الرصيد غير كافي لدفع الدفعة الأولى");

                wallet.Balance -= invoice.DownPayment;

                // تسجيل المعاملة
                _context.Transactions.Add(new Transaction
                {
                    UserId = clientId,
                    Amount = invoice.DownPayment,
                    Type = "InvoicePayment",
                    ReferenceId = invoice.InvoiceNumber,
                    CreatedAt = DateTime.UtcNow
                });

                if (invoice.Installments == null || invoice.Installments.Count == 0)
                {
                    // لا يوجد أقساط → اعتبرها مدفوعة بالكامل
                    invoice.Status = "Paid";
                }
                else
                {
                    // تحديث حالة القسط الأول إذا يساوي الدفعة الأولى
                    var firstInstallment = invoice.Installments.OrderBy(i => i.DueDate).FirstOrDefault();
                    if (firstInstallment != null && firstInstallment.Amount == invoice.DownPayment)
                    {
                        firstInstallment.IsPaid = true;
                        firstInstallment.PaidAt = DateTime.UtcNow;
                    }
                }
            }

            _context.Invoices.Update(invoice);
            return await _context.SaveChangesAsync() > 0;
        }





        public async Task<bool> PayInstallmentAsync(int invoiceId, int installmentId, string clientId)
        {
            var installment = await _context.InvoiceInstallments
                .Include(i => i.Invoice)
                .FirstOrDefaultAsync(i => i.Id == installmentId && i.Invoice.Id == invoiceId && i.Invoice.ClientId == clientId);

            if (installment == null || installment.IsPaid) return false;

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == clientId);
            if (wallet == null || wallet.Balance < installment.Amount) return false;

            // خصم المبلغ
            wallet.Balance -= installment.Amount;
            installment.IsPaid = true;
            installment.PaidAt = DateTime.UtcNow;

            // تسجيل المعاملة
            _context.Transactions.Add(new Transaction
            {
                UserId = clientId,
                Amount = installment.Amount,
                Type = "InvoiceInstallmentPayment",
                ReferenceId = installment.Invoice.InvoiceNumber,
                CreatedAt = DateTime.UtcNow
            });

            // إذا كل الدفعات مدفوعة → غلق الفاتورة كمدفوعة
            var allPaid = await _context.InvoiceInstallments
                .Where(i => i.InvoiceId == invoiceId)
                .AllAsync(i => i.IsPaid);

            if (allPaid)
            {
                installment.Invoice.Status = "Paid";
            }

            await _context.SaveChangesAsync();
            return true;
        }

    }
}





