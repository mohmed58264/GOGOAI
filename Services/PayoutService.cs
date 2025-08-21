
using System;
using System.Linq;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;
using FixoraBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FixoraBackend.Services
{
    public class PayoutService : IPayoutService
    {
        private readonly AppDbContext _context;
        private readonly IWalletService _walletService;

        public PayoutService(AppDbContext context, IWalletService walletService)
        {
            _context = context;
            _walletService = walletService;
        }

        public async Task<PayoutRequest> CreatePayoutRequestAsync(string userId, CreatePayoutRequest request)
        {
            // التحقق من وجود رصيد كافي في المحفظة
            var walletBalance = await _walletService.GetWalletBalanceAsync(userId);
            if (walletBalance < request.Amount)
                throw new InvalidOperationException("الرصيد غير كافي لإتمام عملية السحب.");

            var payout = new PayoutRequest
            {
                UserId = userId,
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod,
                PaymentDetails = request.PaymentDetails,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.PayoutRequests.Add(payout);
            await _context.SaveChangesAsync();

            return payout;
        }

        public async Task<PayoutRequest[]> GetUserPayoutRequestsAsync(string userId)
        {
            return await _context.PayoutRequests
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToArrayAsync();
        }

        public async Task<PayoutRequest[]> GetAllPayoutRequestsAsync()
        {
            return await _context.PayoutRequests
                .OrderByDescending(p => p.CreatedAt)
                .ToArrayAsync();
        }




        public async Task<bool> UpdatePayoutStatusAsync(int payoutId, string status)
        {
            var payout = await _context.PayoutRequests.FindAsync(payoutId);
            if (payout == null) return false;

            payout.Status = status;

            if (status == "Paid")
                payout.PaidAt = DateTime.UtcNow;

            _context.PayoutRequests.Update(payout);
            await _context.SaveChangesAsync();

            // ✅ إرسال إشعار للمستخدم
            await _notificationService.SendPayoutNotificationAsync(
                payout.UserId,
                status,
                payout.Amount
            );

            return true;
        }
        public async Task<int> ProcessBulkPayoutsAsync(BulkPayoutRequest request)
        {
            var payouts = await _context.PayoutRequests
                .Where(p => request.PayoutIds.Contains(p.Id) && p.Status == "Pending")
                .ToListAsync();

            int successCount = 0;

            foreach (var payout in payouts)
            {
                bool result = false;

                if (request.PaymentMethod == "STCPay")
                {
                    result = await _stcPayService.SendPaymentAsync(
                        payout.PaymentDetails,
                        payout.Amount,
                        $"BulkPayout-{payout.Id}"
                    );
                }
                else if (request.PaymentMethod == "BankTransfer")
                {
                    result = await _bankTransferService.SendBankTransferAsync(
                        payout.PaymentDetails,
                        payout.Amount,
                        "Beneficiary Name", // TODO: جلب الاسم من بيانات المستخدم
                        $"BulkPayout-{payout.Id}"
                    );
                }

                if (result)
                {
                    payout.Status = "Paid";
                    payout.PaidAt = DateTime.UtcNow;
                    successCount++;
                }
            }

            await _context.SaveChangesAsync();
            return successCount;
        }





    }
}

