
using System;
using System.Linq;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;
using FixoraBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FixoraBackend.Services
{
    public class SaaSPartnerService : ISaaSPartnerService
    {
        private readonly AppDbContext _context;

        public SaaSPartnerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SaaSPartner> CreatePartnerAsync(CreateSaaSPartnerRequest request)
        {
            var partner = new SaaSPartner
            {
                CompanyName = request.CompanyName,
                OwnerUserId = request.OwnerUserId,
                Country = request.Country,
                City = request.City,
                IsVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.SaaSPartners.Add(partner);
            await _context.SaveChangesAsync();

            return partner;
        }

        public async Task<SaaSPartner[]> GetAllPartnersAsync()
        {
            return await _context.SaaSPartners
                .Include(p => p.Providers)
                .ToArrayAsync();
        }

        public async Task<SaaSPartner> GetPartnerByIdAsync(int partnerId)
        {
            return await _context.SaaSPartners
                .Include(p => p.Providers)
                .FirstOrDefaultAsync(p => p.Id == partnerId);
        }

        public async Task<bool> VerifyPartnerAsync(int partnerId, bool isVerified)
        {
            var partner = await _context.SaaSPartners.FindAsync(partnerId);
            if (partner == null) return false;

            partner.IsVerified = isVerified;
            _context.SaaSPartners.Update(partner);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePartnerAsync(int partnerId)
        {
            var partner = await _context.SaaSPartners.FindAsync(partnerId);
            if (partner == null) return false;

            _context.SaaSPartners.Remove(partner);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

