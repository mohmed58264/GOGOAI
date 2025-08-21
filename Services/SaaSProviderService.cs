
using System;
using System.Linq;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;
using FixoraBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FixoraBackend.Services
{
    public class SaaSProviderService : ISaaSProviderService
    {
        private readonly AppDbContext _context;

        public SaaSProviderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SaaSProvider> CreateProviderAsync(CreateSaaSProviderRequest request)
        {
            var provider = new SaaSProvider
            {
                UserId = request.UserId,
                SaaSPartnerId = request.SaaSPartnerId,
                Specialty = request.Specialty,
                IsActive = true,
                JoinedAt = DateTime.UtcNow
            };

            _context.SaaSProviders.Add(provider);
            await _context.SaveChangesAsync();

            return provider;
        }

        public async Task<SaaSProvider[]> GetProvidersByPartnerIdAsync(int partnerId)
        {
            return await _context.SaaSProviders
                .Where(p => p.SaaSPartnerId == partnerId)
                .ToArrayAsync();
        }

        public async Task<bool> UpdateProviderStatusAsync(int providerId, bool isActive)
        {
            var provider = await _context.SaaSProviders.FindAsync(providerId);
            if (provider == null) return false;

            provider.IsActive = isActive;
            _context.SaaSProviders.Update(provider);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProviderAsync(int providerId)
        {
            var provider = await _context.SaaSProviders.FindAsync(providerId);
            if (provider == null) return false;

            _context.SaaSProviders.Remove(provider);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

