using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;
using FixoraBackend.Services;
using System.Security.Claims;

namespace FixoraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost("approve")]
        [Authorize(Roles = "Client,BusinessClient")]
        public async Task<IActionResult> ApproveOrReject([FromBody] ApproveInvoiceRequest request)
        {
            var userId = User?.Identity?.Name;
            if (userId == null) return Unauthorized();

            var result = await _invoiceService.ApproveOrRejectInvoiceAsync(request, userId);
            if (!result) return BadRequest("Unable to update invoice status");

            return Ok(new { message = "Invoice status updated successfully" });
        }




        // ✅ إنشاء فاتورة (مزود الخدمة فقط)
        [HttpPost("create")]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceRequest request)
        {
            var providerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(providerId))
                return Unauthorized();

            // يمكن الحصول على ClientId من الطلب نفسه أو تمريره في الـ DTO
            var clientId = ""; // TODO: جلبه من OrderService
            var invoice = await _invoiceService.CreateInvoiceAsync(providerId, request, clientId);

            return Ok(invoice);
        }

        // ✅ موافقة العميل على الفاتورة
        [HttpPost("{invoiceId}/approve")]
        [Authorize(Roles = "Client,BusinessClient")]
        public async Task<IActionResult> ApproveInvoice(int invoiceId)
        {
            var result = await _invoiceService.UpdateInvoiceStatusAsync(invoiceId, "Approved");
            if (!result) return NotFound(new { message = "الفاتورة غير موجودة" });

            return Ok(new { message = "تمت الموافقة على الفاتورة" });
        }

        // ✅ رفض العميل للفاتورة
        [HttpPost("{invoiceId}/reject")]
        [Authorize(Roles = "Client,BusinessClient")]
        public async Task<IActionResult> RejectInvoice(int invoiceId)
        {
            var result = await _invoiceService.UpdateInvoiceStatusAsync(invoiceId, "Rejected");
            if (!result) return NotFound(new { message = "الفاتورة غير موجودة" });

            return Ok(new { message = "تم رفض الفاتورة" });
        }

        // ✅ عرض فواتير طلب معين
        [HttpGet("order/{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetInvoicesForOrder(string orderId)
        {
            var invoices = await _invoiceService.GetInvoicesByOrderAsync(orderId);
            return Ok(invoices);
        }

        // ✅ عرض جميع فواتير المستخدم
        [HttpGet("my-invoices")]
        [Authorize]
        public async Task<IActionResult> GetMyInvoices()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var invoices = await _invoiceService.GetInvoicesByUserAsync(userId);
            return Ok(invoices);
        }




        [HttpPost("{invoiceId}/approve-and-pay")]
        [Authorize(Roles = "Client,BusinessClient")]
        public async Task<IActionResult> ApproveAndPayInvoice(int invoiceId)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var result = await _invoiceService.ApproveAndPayInvoiceAsync(invoiceId, clientId);
                if (!result) return NotFound(new { message = "الفاتورة غير موجودة" });

                return Ok(new { message = "تمت الموافقة على الفاتورة ودفع الدفعة الأولى" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }




        [HttpPost("{invoiceId}/pay-installment/{installmentId}")]
        [Authorize(Roles = "Client,BusinessClient")]
        public async Task<IActionResult> PayInstallment(int invoiceId, int installmentId)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _invoiceService.PayInstallmentAsync(invoiceId, installmentId, clientId);
            if (!result) return BadRequest(new { message = "لا يمكن دفع هذه الدفعة" });

            return Ok(new { message = "تم دفع الدفعة بنجاح" });
        }
    }
}


