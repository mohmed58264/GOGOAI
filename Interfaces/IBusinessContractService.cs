
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Models;

namespace FixoraBackend.Interfaces
{
    public interface IBusinessContractService
    {
        Task<BusinessContract> CreateContractAsync(CreateBusinessContractRequest request);

        Task<BusinessContract> GetContractByClientIdAsync(int businessClientId);
    }
}
