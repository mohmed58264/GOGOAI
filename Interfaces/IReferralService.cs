
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Models;

namespace FixoraBackend.Interfaces
{
    public interface IReferralService
    {
        Task<bool> RegisterReferralAsync(CreateReferralCodeRequest request);

        Task<ReferralStatusResponse> GetReferralStatusAsync(string userId);

        Task<bool> AddReferralEarningAsync(string referrerUserId, int orderId, decimal amount);
        
        Task<Referral> CreateReferralAsync(CreateReferralRequest request);

        Task<Referral[]> GetReferralsByUserAsync(string userId);

        Task<bool> AddCommissionAsync(int referralId, decimal amount);

        Task<bool> MarkCommissionAsPaidAsync(int referralId);
    }
}