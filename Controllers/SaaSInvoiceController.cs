
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;

namespace FixoraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaaSInvoiceController : ControllerBase
    {
        private readonly ISaaSInvoiceService _invoiceService;

        public SaaSInvoiceController(ISaaSInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // ⬅️ إنشاء فاتورة جديدة
        [HttpPost("create")]
        [Authorize(Roles = "Admin,PartnerAdmin")]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateSaaSInvoiceRequest request)
        {
            var invoice = await _invoiceService.CreateInvoiceAsync(request);
            return Ok(invoice);
        }

        // ⬅️ عرض جميع الفواتير لشركة SaaS
        [HttpGet("partner/{partnerId}")]
        [Authorize(Roles = "Admin,PartnerAdmin")]
        public async Task<IActionResult> GetInvoicesByPartner(int partnerId)
        {
            var invoices = await _invoiceService.GetInvoicesByPartnerIdAsync(partnerId);
            return Ok(invoices);
        }

        // ⬅️ تحديد الفاتورة كمسددة
        [HttpPost("{invoiceId}/mark-paid")]
        [Authorize(Roles = "Admin,PartnerAdmin")]
        public async Task<IActionResult> MarkAsPaid(int invoiceId)
        {
            var result = await _invoiceService.MarkInvoiceAsPaidAsync(invoiceId);
            if (!result) return NotFound();
            return Ok(new { message = "Invoice marked as paid" });
        }

        // ⬅️ حذف فاتورة
        [HttpDelete("{invoiceId}")]
        [Authorize(Roles = "Admin,PartnerAdmin")]
        public async Task<IActionResult> DeleteInvoice(int invoiceId)
        {
            var result = await _invoiceService.DeleteInvoiceAsync(invoiceId);
            if (!result) return NotFound();
            return Ok(new { message = "Invoice deleted" });
        }
    }
}

