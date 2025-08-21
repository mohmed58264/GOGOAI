
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FixoraBackend.Data;
using FixoraBackend.Models;

namespace FixoraBackend.Services
{
    public class SupervisorService
    {
        private readonly AppDbContext _context;

        public SupervisorService(AppDbContext context)
        {
            _context = context;
        }


        // ربط مزود خدمة بالسوبرفايزر
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




        public async Task<Provider> RegisterProviderAsync(Provider provider, string supervisorCode)
        {
            Supervisor supervisor = null;

            if (!string.IsNullOrEmpty(supervisorCode))
            {
                supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.Code == supervisorCode);
            }

            // إذا ما كتب الكود أو الكود غير صحيح → سوبرفايزر الإدارة (100)
            if (supervisor == null)
            {
                supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.Code == "100");
            }

            provider.SupervisorId = supervisor.Id;

            _context.Providers.Add(provider);
            await _context.SaveChangesAsync();

            return provider;
        }


        public async Task<bool> ChangeProviderSupervisorAsync(int providerId, int newSupervisorId, string requesterRole)
        {
            // فقط الأدمن يستطيع تغيير السوبرفايزر
            if (requesterRole != "Admin")
                return false;

            var provider = await _context.Providers.FindAsync(providerId);
            if (provider == null)
                return false;

            var newSupervisor = await _context.Supervisors.FindAsync(newSupervisorId);
            if (newSupervisor == null)
                return false;

            provider.SupervisorId = newSupervisorId;
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Supervisor> CreateSupervisorAsync(string userId)
        {
            var random = new Random();
            string code;

            do
            {
                code = random.Next(100, 999).ToString(); // كود من 3 أرقام
            }
            while (await _context.Supervisors.AnyAsync(s => s.Code == code));

            var supervisor = new Supervisor
            {
                UserId = userId,
                Code = code
            };

            _context.Supervisors.Add(supervisor);
            await _context.SaveChangesAsync();

            return supervisor;
        }
    }
}