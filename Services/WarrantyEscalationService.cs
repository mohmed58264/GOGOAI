
using System;
using System.Linq;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;
using FixoraBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FixoraBackend.Services
{
    public class WarrantyEscalationService : IWarrantyEscalationService
    {
        private readonly AppDbContext _context;

        public WarrantyEscalationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<WarrantyEscalation> EscalateAsync(EscalateWarrantyRequest request, string userId)
        {
            var warranty = await _context.Warranties
                .FirstOrDefaultAsync(w => w.Id == request.WarrantyId);

            if (warranty == null)
                throw new Exception("Warranty not found");

            var escalation = new WarrantyEscalation
            {
                WarrantyId = request.WarrantyId,
                EscalatedByUserId = userId,
                EscalationLevel = request.EscalationLevel,
                Reason = request.Reason,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.WarrantyEscalations.Add(escalation);
            await _context.SaveChangesAsync();

            return escalation;
        }

        public async Task<bool> ResolveEscalationAsync(int escalationId, string resolutionStatus)
        {
            var escalation = await _context.WarrantyEscalations
                .FirstOrDefaultAsync(e => e.Id == escalationId);

            if (escalation == null)
                return false;

            escalation.Status = resolutionStatus;
            escalation.ResolvedAt = DateTime.UtcNow;

            _context.WarrantyEscalations.Update(escalation);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<WarrantyEscalation[]> GetEscalationsByWarrantyIdAsync(int warrantyId)
        {
            return await _context.WarrantyEscalations
                .Where(e => e.WarrantyId == warrantyId)
                .OrderByDescending(e => e.CreatedAt)
                .ToArrayAsync();
        }
    }
}

