
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;

namespace FixoraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayoutController : ControllerBase
    {
        private readonly IPayoutService _payoutService;

        public PayoutController(IPayoutService payoutService)
        {
            _payoutService = payoutService;
        }

        // ⬅️ إنشاء طلب سحب جديد (للمستخدم)
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreatePayout([FromBody] CreatePayoutRequest request)
        {
            var userId = User.FindFirst("sub")?.Value; // جلب معرف المستخدم من الـ JWT
            var payout = await _payoutService.CreatePayoutRequestAsync(userId, request);
            return Ok(payout);
        }

        // ⬅️ عرض طلبات السحب الخاصة بالمستخدم
        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyPayouts()
        {
            var userId = User.FindFirst("sub")?.Value;
            var payouts = await _payoutService.GetUserPayoutRequestsAsync(userId);
            return Ok(payouts);
        }

        // ⬅️ عرض جميع طلبات السحب (للأدمن)
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPayouts()
        {
            var payouts = await _payoutService.GetAllPayoutRequestsAsync();
            return Ok(payouts);
        }

        // ⬅️ تحديث حالة طلب السحب (موافقة، رفض، مدفوع)
        [HttpPost("{payoutId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePayoutStatus(int payoutId, [FromQuery] string status)
        {
            var result = await _payoutService.UpdatePayoutStatusAsync(payoutId, status);
            if (!result) return NotFound();
            return Ok(new { message = "تم تحديث حالة طلب السحب بنجاح" });
        }

        [HttpPost("bulk-pay")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProcessBulkPayouts([FromBody] BulkPayoutRequest request)
        {
            var count = await _payoutService.ProcessBulkPayoutsAsync(request);
            return Ok(new { message = $"تم دفع {count} طلبات سحب بنجاح" });
        }

    }
}

