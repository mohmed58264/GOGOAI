
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Models;

namespace FixoraBackend.Interfaces
{
    public interface IWarrantyEscalationService
    {
        Task<WarrantyEscalation> EscalateAsync(EscalateWarrantyRequest request, string userId);

        Task<bool> ResolveEscalationAsync(int escalationId, string resolutionStatus);

        Task<WarrantyEscalation[]> GetEscalationsByWarrantyIdAsync(int warrantyId);
    }
}

