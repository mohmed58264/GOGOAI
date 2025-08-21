
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;

namespace FixoraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicePaymentController : ControllerBase
    {
        private readonly IInvoicePaymentService _paymentService;

        public InvoicePaymentController(IInvoicePaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("pay")]
        [Authorize(Roles = "Client,BusinessClient")]
        public async Task<IActionResult> PayInvoice([FromBody] CreateInvoicePaymentRequest request)
        {
            var userId = User?.Identity?.Name;
            if (userId == null)
                return Unauthorized();

            var result = await _paymentService.CreatePaymentAsync(request, userId);
            if (!result)
                return BadRequest("Unable to process payment");

            return Ok(new { message = "Payment recorded successfully" });
        }
    }
}

