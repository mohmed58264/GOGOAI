using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class WarrantyController : ControllerBase
{
    private readonly WarrantyService _service;

    public WarrantyController(WarrantyService service)
    {
        _service = service;
    }

    [HttpPost("submit")]
    [Authorize]
    public async Task<IActionResult> Submit([FromBody] WarrantyRequest request)
    {
        await _service.SubmitAsync(request);
        return Ok(new { success = true });
    }

    [HttpPost("review")]
    [Authorize(Roles = "provider")]
    public async Task<IActionResult> ProviderReview([FromBody] WarrantyReview review)
    {
        await _service.ProviderReviewAsync(review);
        return Ok(new { success = true });
    }

    [HttpPost("escalate")]
    [Authorize(Roles = "supervisor")]
    public async Task<IActionResult> Escalate([FromBody] WarrantyEscalation escalation)
    {
        await _service.EscalateToSupervisorAsync(escalation);
        return Ok(new { success = true });
    }
}
