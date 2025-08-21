
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;

namespace FixoraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarrantyEscalationController : ControllerBase
    {
        private readonly IWarrantyEscalationService _escalationService;

        public WarrantyEscalationController(IWarrantyEscalationService escalationService)
        {
            _escalationService = escalationService;
        }

        // ⬅️ تسجيل تصعيد جديد
        [HttpPost("escalate")]
        [Authorize]
        public async Task<IActionResult> Escalate([FromBody] EscalateWarrantyRequest request)
        {
            var userId = User?.Identity?.Name;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var escalation = await _escalationService.EscalateAsync(request, userId);
            return Ok(escalation);
        }

        // ⬅️ حل التصعيد (يتم من قبل الإدارة أو المشرفين)
        [HttpPost("{escalationId}/resolve")]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Resolve(int escalationId, [FromQuery] string status)
        {
            var result = await _escalationService.ResolveEscalationAsync(escalationId, status);
            if (!result) return NotFound("Escalation not found");

            return Ok(new { message = "Escalation resolved", status });
        }

        // ⬅️ عرض كل التصعيدات الخاصة بضمان معيّن
        [HttpGet("warranty/{warrantyId}")]
        [Authorize]
        public async Task<IActionResult> GetByWarranty(int warrantyId)
        {
            var escalations = await _escalationService.GetEscalationsByWarrantyIdAsync(warrantyId);
            return Ok(escalations);
        }
    }
}
