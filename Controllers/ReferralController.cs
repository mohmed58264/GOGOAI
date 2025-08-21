
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;

namespace FixoraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReferralController : ControllerBase
    {
        private readonly IReferralService _referralService;

        public ReferralController(IReferralService referralService)
        {
            _referralService = referralService;
        }

        // ⬅️ تسجيل كود الإحالة من مستخدم جديد
        [HttpPost("register")]
        [Authorize]
        public async Task<IActionResult> RegisterReferral([FromBody] CreateReferralCodeRequest request)
        {
            var result = await _referralService.RegisterReferralAsync(request);
            if (!result)
                return BadRequest("Referral registration failed. Check the referral code.");

            return Ok(new { message = "Referral registered successfully" });
        }

        // ⬅️ استعلام حالة الإحالة والمكافآت
        [HttpGet("status")]
        [Authorize]
        public async Task<IActionResult> GetReferralStatus()
        {
            var userId = User?.Identity?.Name;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var status = await _referralService.GetReferralStatusAsync(userId);
            return Ok(status);
        }


        // ⬅️ إنشاء إحالة جديدة
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateReferral([FromBody] CreateReferralRequest request)
        {
            var referral = await _referralService.CreateReferralAsync(request);
            return Ok(referral);
        }

        // ⬅️ عرض كل الإحالات لمستخدم
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetReferralsByUser(string userId)
        {
            var referrals = await _referralService.GetReferralsByUserAsync(userId);
            return Ok(referrals);
        }

        // ⬅️ إضافة عمولة للإحالة
        [HttpPost("{referralId}/add-commission")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCommission(int referralId, [FromQuery] decimal amount)
        {
            var result = await _referralService.AddCommissionAsync(referralId, amount);
            if (!result) return NotFound();
            return Ok(new { message = "Commission added successfully" });
        }

        // ⬅️ تحديد العمولة كمدفوعة
        [HttpPost("{referralId}/mark-paid")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MarkCommissionAsPaid(int referralId)
        {
            var result = await _referralService.MarkCommissionAsPaidAsync(referralId);
            if (!result) return NotFound();
            return Ok(new { message = "Commission marked as paid" });
        }
    }
}




