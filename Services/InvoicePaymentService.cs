
using System;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;
using FixoraBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FixoraBackend.Services
{
    public class InvoicePaymentService : IInvoicePaymentService
    {
        private readonly AppDbContext _context;

        public InvoicePaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreatePaymentAsync(CreateInvoicePaymentRequest request, string userId)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(i => i.Id == request.InvoiceId);

            if (invoice == null || invoice.Status == "Rejected")
                return false;

            var totalPaid = invoice.Payments.Sum(p => p.AmountPaid);
            var remaining = invoice.TotalAmount - totalPaid;

            if (request.Amount > remaining || request.Amount <= 0)
                return false;

            var payment = new InvoicePayment
            {
                InvoiceId = invoice.Id,
                AmountPaid = request.Amount,
                PaymentMethod = request.PaymentMethod,
                PaidAt = DateTime.UtcNow,
                PaidByUserId = userId
            };

            _context.InvoicePayments.Add(payment);

            if (invoice.RequiresFirstPayment && !invoice.FirstPaymentPaid)
            {
                if (request.Amount >= invoice.FirstPaymentAmount)
                {
                    invoice.FirstPaymentPaid = true;
                    if (invoice.Status == "AwaitingFirstPayment")
                        invoice.Status = "Approved";

                    invoice.StatusHistory.Add(new InvoiceStatusHistory
                    {
                        InvoiceId = invoice.Id,
                        Status = invoice.Status,
                        ChangedByUserId = userId,
                        Note = "First payment completed",
                        ChangedAt = DateTime.UtcNow
                    });
                }
            }

            if ((totalPaid + request.Amount) >= invoice.TotalAmount)
            {
                invoice.StatusHistory.Add(new InvoiceStatusHistory
                {
                    InvoiceId = invoice.Id,
                    Status = "PaidInFull",
                    ChangedByUserId = userId,
                    Note = "Invoice fully paid",
                    ChangedAt = DateTime.UtcNow
                });
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
