using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Models;

namespace FixoraBackend.Interfaces
{
    public interface IPayoutService
    {
        Task<PayoutRequest> CreatePayoutRequestAsync(string userId, CreatePayoutRequest request);

        Task<PayoutRequest[]> GetUserPayoutRequestsAsync(string userId);

        Task<PayoutRequest[]> GetAllPayoutRequestsAsync();

        Task<bool> UpdatePayoutStatusAsync(int payoutId, string status);
    }
}

  