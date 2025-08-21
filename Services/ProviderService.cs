using Microsoft.EntityFrameworkCore;

namespace FixoraBackend.Services
{
    public class ProviderService
    {
        

public async Task<Provider> RegisterProviderAsync(Provider provider, string supervisorCode)
        {
            if (!string.IsNullOrEmpty(supervisorCode))
            {
                var supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.Code == supervisorCode);
                if (supervisor != null)
                {
                    provider.SupervisorId = supervisor.Id;
                }
            }

            _context.Providers.Add(provider);
            await _context.SaveChangesAsync();

            return provider;
        }


    }
}
