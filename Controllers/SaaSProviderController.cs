
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;

namespace FixoraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaaSProviderController : ControllerBase
    {
        private readonly ISaaSProviderService _providerService;

        public SaaSProviderController(ISaaSProviderService providerService)
        {
            _providerService = providerService;
        }

        // ⬅️ إضافة مزود جديد
        [HttpPost("create")]
        [Authorize(Roles = "Admin,PartnerAdmin")]
        public async Task<IActionResult> CreateProvider([FromBody] CreateSaaSProviderRequest request)
        {
            var provider = await _providerService.CreateProviderAsync(request);
            return Ok(provider);
        }

        // ⬅️ عرض كل المزودين لشركة SaaS معينة
        [HttpGet("partner/{partnerId}")]
        [Authorize(Roles = "Admin,PartnerAdmin")]
        public async Task<IActionResult> GetProvidersByPartner(int partnerId)
        {
            var providers = await _providerService.GetProvidersByPartnerIdAsync(partnerId);
            return Ok(providers);
        }

        // ⬅️ تغيير حالة مزود (تفعيل/إلغاء)
        [HttpPost("{providerId}/status")]
        [Authorize(Roles = "Admin,PartnerAdmin")]
        public async Task<IActionResult> UpdateStatus(int providerId, [FromQuery] bool isActive)
        {
            var result = await _providerService.UpdateProviderStatusAsync(providerId, isActive);
            if (!result) return NotFound();
            return Ok(new { message = "Provider status updated" });
        }

        // ⬅️ حذف مزود
        [HttpDelete("{providerId}")]
        [Authorize(Roles = "Admin,PartnerAdmin")]
        public async Task<IActionResult> DeleteProvider(int providerId)
        {
            var result = await _providerService.DeleteProviderAsync(providerId);
            if (!result) return NotFound();
            return Ok(new { message = "Provider deleted" });
        }
    }
}

