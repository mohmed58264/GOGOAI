
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Models;

namespace FixoraBackend.Interfaces
{
    public interface ISaaSProviderService
    {
        Task<SaaSProvider> CreateProviderAsync(CreateSaaSProviderRequest request);

        Task<SaaSProvider[]> GetProvidersByPartnerIdAsync(int partnerId);

        Task<bool> UpdateProviderStatusAsync(int providerId, bool isActive);

        Task<bool> DeleteProviderAsync(int providerId);
    }
}

