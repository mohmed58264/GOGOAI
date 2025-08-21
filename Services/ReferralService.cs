
using System;
using System.Linq;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;
using FixoraBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FixoraBackend.Services
{
    public class ReferralService : IReferralService
    {
        private readonly AppDbContext _context;
        private readonly IWalletService _walletService; // ✅ إضافة خدمة المحفظة

        public ReferralService(AppDbContext context, IWalletService walletService)
        {
            _context = context;
            _walletService = walletService;
        }

        public async Task<bool> RegisterReferralAsync(CreateReferralCodeRequest request)
        {
            var referrer = await _context.Users
                .FirstOrDefaultAsync(u => u.ReferralCode == request.ReferrerCode);

            if (referrer == null || request.ReferredUserId == referrer.Id)
                return false;

            var exists = await _context.UserReferrals
                .AnyAsync(r => r.ReferredUserId == request.ReferredUserId);

            if (exists)
                return false;

            var referral = new UserReferral
            {
                ReferrerUserId = referrer.Id,
                ReferredUserId = request.ReferredUserId,
                ReferredAt = DateTime.UtcNow,
                IsVerified = true
            };

            _context.UserReferrals.Add(referral);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ReferralStatusResponse> GetReferralStatusAsync(string userId)
        {
            var totalEarnings = await _context.ReferralEarnings
                .Where(e => e.ReferrerUserId == userId)
                .SumAsync(e => e.Amount);

            var totalReferred = await _context.UserReferrals
                .CountAsync(r => r.ReferrerUserId == userId);

            var referredBy = await _context.UserReferrals
                .Where(r => r.ReferredUserId == userId)
                .Select(r => r.ReferrerUserId)
                .FirstOrDefaultAsync();

            return new ReferralStatusResponse
            {
                TotalEarnings = totalEarnings,
                TotalReferredUsers = totalReferred,
                ReferredByUserId = referredBy,
                IsReferred = referredBy != null
            };
        }

        public async Task<bool> AddReferralEarningAsync(string referrerUserId, int orderId, decimal amount)
        {
            var earning = new ReferralEarning
            {
                ReferrerUserId = referrerUserId,
                RelatedOrderId = orderId,
                Amount = amount,
                EarnedAt = DateTime.UtcNow
            };

            _context.ReferralEarnings.Add(earning);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<Referral> CreateReferralAsync(CreateReferralRequest request)
        {
            var referral = new Referral
            {
                ReferrerUserId = request.ReferrerUserId,
                ReferredUserId = request.ReferredUserId,
                ReferredAt = DateTime.UtcNow,
                CommissionEarned = 0m,
                IsCommissionPaid = false
            };

            _context.Referrals.Add(referral);
            await _context.SaveChangesAsync();

            return referral;
        }

        public async Task<Referral[]> GetReferralsByUserAsync(string userId)
        {
            var query = _context.Referrals.AsQueryable();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(r => r.ReferrerUserId == userId);

            return await query.OrderByDescending(r => r.ReferredAt).ToArrayAsync();
        }

        public async Task<bool> AddCommissionAsync(int referralId, decimal amount)
        {
            var referral = await _context.Referrals.FindAsync(referralId);
            if (referral == null) return false;

            referral.CommissionEarned += amount;
            _context.Referrals.Update(referral);

            // ✅ إيداع العمولة فورًا في المحفظة
            await _walletService.AddTransactionAsync(new WalletTransactionRequest
            {
                UserId = referral.ReferrerUserId,
                Amount = amount,
                Description = "عمولة إحالة",
                TransactionType = TransactionType.ReferralCommission
            });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> MarkCommissionAsPaidAsync(int referralId)
        {
            var referral = await _context.Referrals.FindAsync(referralId);
            if (referral == null) return false;

            referral.IsCommissionPaid = true;
            _context.Referrals.Update(referral);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}




