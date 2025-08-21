
using System.Threading.Tasks;
using FixoraBackend.DTOs;

namespace FixoraBackend.Interfaces
{
    public interface IProviderBadgeService
    {
        Task<bool> AwardBadgeAsync(AwardBadgeRequest request);

        Task<bool> RemoveExpiredBadgesAsync();

        Task<string[]> GetActiveBadgesForProviderAsync(string providerUserId);
    }
}

