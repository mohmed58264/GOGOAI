
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;

namespace FixoraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProviderBadgeController : ControllerBase
    {
        private readonly IProviderBadgeService _badgeService;

        public ProviderBadgeController(IProviderBadgeService badgeService)
        {
            _badgeService = badgeService;
        }

        // ⬅️ منح شارة لمزود الخدمة
        [HttpPost("award")]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> AwardBadge([FromBody] AwardBadgeRequest request)
        {
            var result = await _badgeService.AwardBadgeAsync(request);
            if (!result)
                return BadRequest("Failed to award badge");

            return Ok(new { message = "Badge awarded successfully" });
        }

        // ⬅️ عرض الشارات النشطة لمزود
        [HttpGet("{providerUserId}/active")]
        [Authorize]
        public async Task<IActionResult> GetActiveBadges(string providerUserId)
        {
            var badges = await _badgeService.GetActiveBadgesForProviderAsync(providerUserId);
            return Ok(badges);
        }

        // ⬅️ تنظيف الشارات المنتهية (تشغيلها يدويًا أو من Scheduler)
        [HttpDelete("cleanup")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CleanupExpiredBadges()
        {
            var result = await _badgeService.RemoveExpiredBadgesAsync();
            return result
                ? Ok(new { message = "Expired badges removed successfully" })
                : Ok(new { message = "No expired badges found" });
        }
    }
}