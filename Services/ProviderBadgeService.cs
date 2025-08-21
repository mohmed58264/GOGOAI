
using System;
using System.Linq;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;
using FixoraBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FixoraBackend.Services
{
    public class ProviderBadgeService : IProviderBadgeService
    {
        private readonly AppDbContext _context;

        public ProviderBadgeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AwardBadgeAsync(AwardBadgeRequest request)
        {
            var badge = new ProviderBadge
            {
                ProviderUserId = request.ProviderUserId,
                BadgeName = request.BadgeName,
                AwardedAt = DateTime.UtcNow,
                ExpiryDate = request.ExpiryDate
            };

            _context.ProviderBadges.Add(badge);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveExpiredBadgesAsync()
        {
            var expiredBadges = await _context.ProviderBadges
                .Where(b => b.ExpiryDate < DateTime.UtcNow)
                .ToListAsync();

            if (!expiredBadges.Any())
                return false;

            _context.ProviderBadges.RemoveRange(expiredBadges);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<string[]> GetActiveBadgesForProviderAsync(string providerUserId)
        {
            return await _context.ProviderBadges
                .Where(b => b.ProviderUserId == providerUserId && b.ExpiryDate >= DateTime.UtcNow)
                .Select(b => b.BadgeName)
                .ToArrayAsync();
        }
    }
}

