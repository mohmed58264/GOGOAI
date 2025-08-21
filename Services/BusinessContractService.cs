
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;
using FixoraBackend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FixoraBackend.Services
{
    public class BusinessContractService : IBusinessContractService
    {
        private readonly AppDbContext _context;

        public BusinessContractService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessContract> CreateContractAsync(CreateBusinessContractRequest request)
        {
            var contract = new BusinessContract
            {
                BusinessClientId = request.BusinessClientId,
                ContractType = request.ContractType,
                FixedAmountPerPeriod = request.FixedAmountPerPeriod,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Notes = request.Notes,
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };

            _context.BusinessContracts.Add(contract);
            await _context.SaveChangesAsync();
            return contract;
        }

        public async Task<BusinessContract> GetContractByClientIdAsync(int businessClientId)
        {
            return await _context.BusinessContracts
                .FirstOrDefaultAsync(c => c.BusinessClientId == businessClientId && c.Status == "Active");
        }
    }
}
