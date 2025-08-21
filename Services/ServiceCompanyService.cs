
using System;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;
using FixoraBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FixoraBackend.Services
{
    public class ServiceCompanyService : IServiceCompanyService
    {
        private readonly AppDbContext _context;

        public ServiceCompanyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceCompany> RegisterCompanyAsync(CompanyRegistrationRequest request, string userId)
        {
            var company = new ServiceCompany
            {
                CompanyName = request.CompanyName,
                CommercialNumber = request.CommercialNumber,
                Country = request.Country,
                City = request.City,
                Phone = request.Phone,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow
            };

            _context.ServiceCompanies.Add(company);
            await _context.SaveChangesAsync();

            var linkUser = new CompanyUser
            {
                UserId = userId,
                ServiceCompanyId = company.Id,
                Role = "Admin",
                AssignedAt = DateTime.UtcNow
            };

            _context.CompanyUsers.Add(linkUser);
            await _context.SaveChangesAsync();

            return company;
        }

        public async Task<ServiceCompany> GetCompanyByIdAsync(int id)
        {
            return await _context.ServiceCompanies.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> AssignUserToCompanyAsync(string userId, int companyId, string role)
        {
            var existing = await _context.CompanyUsers
                .FirstOrDefaultAsync(u => u.UserId == userId && u.ServiceCompanyId == companyId);

            if (existing != null) return false;

            var link = new CompanyUser
            {
                UserId = userId,
                ServiceCompanyId = companyId,
                Role = role,
                AssignedAt = DateTime.UtcNow
            };

            _context.CompanyUsers.Add(link);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

