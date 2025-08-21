
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Models;

namespace FixoraBackend.Interfaces
{
    public interface IServiceCompanyService
    {
        Task<ServiceCompany> RegisterCompanyAsync(CompanyRegistrationRequest request, string userId);

        Task<ServiceCompany> GetCompanyByIdAsync(int id);

        Task<bool> AssignUserToCompanyAsync(string userId, int companyId, string role);
    }
}
