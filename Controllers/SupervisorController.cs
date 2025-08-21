
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FixoraBackend.Data;
using FixoraBackend.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FixoraBackend.Controllers
{
    [Authorize(Roles = "Supervisor")]
    [ApiController]
    [Route("api/[controller]")]
    public class SupervisorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SupervisorController(AppDbContext context)
        {
            _context = context;
        }

        // عرض قائمة مزودي الخدمة التابعين للسوبرفايزر
        [HttpGet("providers")]
        public async Task<IActionResult> GetMyProviders()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.UserId == userId);

            if (supervisor == null)
                return Unauthorized(new { message = "Supervisor not found" });

            var providers = await _context.Providers
                .Where(p => p.SupervisorId == supervisor.Id)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.IsActive,
                    p.Specializations
                })
                .ToListAsync();

            return Ok(providers);
        }

        // تفعيل أو إيقاف مزود خدمة
        [HttpPost("providers/{providerId}/toggle-activation")]
        public async Task<IActionResult> ToggleProviderActivation(int providerId)
        {
            var provider = await _context.Providers.FirstOrDefaultAsync(p => p.Id == providerId);
            if (provider == null)
                return NotFound(new { message = "Provider not found" });

            provider.IsActive = !provider.IsActive;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Provider {(provider.IsActive ? "activated" : "deactivated")} successfully" });
        }

        // تعديل مهن مزود الخدمة
        [HttpPost("providers/{providerId}/update-skills")]
        public async Task<IActionResult> UpdateProviderSkills(int providerId, [FromBody] string[] skills)
        {
            var provider = await _context.Providers.FirstOrDefaultAsync(p => p.Id == providerId);
            if (provider == null)
                return NotFound(new { message = "Provider not found" });

            provider.Specializations = string.Join(",", skills);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Provider skills updated successfully" });
        }

        // عرض الطلبات الجارية
        [HttpGet("orders/active")]
        public async Task<IActionResult> GetActiveOrders()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.UserId == userId);

            if (supervisor == null)
                return Unauthorized(new { message = "Supervisor not found" });

            var orders = await _context.Orders
                .Where(o => o.Provider.SupervisorId == supervisor.Id && o.Status == "InProgress")
                .ToListAsync();

            return Ok(orders);
        }

        // عرض الطلبات المكتملة
        [HttpGet("orders/completed")]
        public async Task<IActionResult> GetCompletedOrders()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.UserId == userId);

            if (supervisor == null)
                return Unauthorized(new { message = "Supervisor not found" });

            var orders = await _context.Orders
                .Where(o => o.Provider.SupervisorId == supervisor.Id && o.Status == "Completed")
                .ToListAsync();

            return Ok(orders);
        }

        // عرض الطلبات التي عليها ضمان
        [HttpGet("orders/warranty")]
        public async Task<IActionResult> GetWarrantyOrders()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.UserId == userId);

            if (supervisor == null)
                return Unauthorized(new { message = "Supervisor not found" });

            var orders = await _context.Orders
                .Where(o => o.Provider.SupervisorId == supervisor.Id && o.Status == "WarrantyOpened")
                .ToListAsync();

            return Ok(orders);
        }

        // عرض أرباح السوبرفايزر من عمولة الطلبات
        [HttpGet("earnings")]
        public async Task<IActionResult> GetSupervisorEarnings()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.UserId == userId);

            if (supervisor == null)
                return Unauthorized(new { message = "Supervisor not found" });

            var earnings = await _context.Orders
                .Where(o => o.Provider.SupervisorId == supervisor.Id && o.Status == "Completed")
                .SumAsync(o => o.TotalAmount * supervisor.CommissionRate);

            return Ok(new { totalEarnings = earnings });
        }
    }
}

