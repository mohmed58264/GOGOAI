
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;

namespace FixoraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaaSPartnerController : ControllerBase
    {
        private readonly ISaaSPartnerService _partnerService;

        public SaaSPartnerController(ISaaSPartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        // ⬅️ إنشاء شريك SaaS جديد
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePartner([FromBody] CreateSaaSPartnerRequest request)
        {
            var partner = await _partnerService.CreatePartnerAsync(request);
            return Ok(partner);
        }

        // ⬅️ عرض كل شركاء SaaS
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPartners()
        {
            var partners = await _partnerService.GetAllPartnersAsync();
            return Ok(partners);
        }

        // ⬅️ عرض تفاصيل شريك واحد
        [HttpGet("{partnerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPartnerById(int partnerId)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(partnerId);
            if (partner == null) return NotFound();
            return Ok(partner);
        }

        // ⬅️ توثيق أو إلغاء توثيق شريك
        [HttpPost("{partnerId}/verify")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> VerifyPartner(int partnerId, [FromQuery] bool isVerified)
        {
            var result = await _partnerService.VerifyPartnerAsync(partnerId, isVerified);
            if (!result) return NotFound();
            return Ok(new { message = "Partner verification updated" });
        }

        // ⬅️ حذف شريك SaaS
        [HttpDelete("{partnerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePartner(int partnerId)
        {
            var result = await _partnerService.DeletePartnerAsync(partnerId);
            if (!result) return NotFound();
            return Ok(new { message = "Partner deleted" });
        }
    }
}

