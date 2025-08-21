
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Models;

namespace FixoraBackend.Interfaces
{
    public interface ISaaSPartnerService
    {
        Task<SaaSPartner> CreatePartnerAsync(CreateSaaSPartnerRequest request);

        Task<SaaSPartner[]> GetAllPartnersAsync();

        Task<SaaSPartner> GetPartnerByIdAsync(int partnerId);

        Task<bool> VerifyPartnerAsync(int partnerId, bool isVerified);

        Task<bool> DeletePartnerAsync(int partnerId);
    }
}

