using FixoraBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FixoraBackend.Controllers
{
    public class AdminController
    {
        [Authorize(Roles = "Admin,Supervisor")]
        [HttpGet("provider/{providerId}/location-history")]
        public async Task<IActionResult> GetProviderLocationHistory(string providerId)
        {
            var history = await _context.ProviderLocationHistories
                .Where(h => h.ProviderId == providerId)
                .OrderByDescending(h => h.RecordedAt)
                .ToListAsync();

            return Ok(history);
        }


        // إنشاء سوبرفايزر جديد
        [HttpPost("create-supervisor")]
        public async Task<IActionResult> CreateSupervisor([FromBody] string userId)
        {
            var supervisor = await _supervisorService.CreateSupervisorAsync(userId);

            if (supervisor == null)
                return BadRequest(new { message = "Failed to create supervisor" });

            return Ok(new
            {
                message = "Supervisor created successfully",
                supervisorId = supervisor.Id,
                code = supervisor.Code
            });
        }

    }
}
