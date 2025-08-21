using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PricingController : ControllerBase
{
    private readonly PricingService _pricingService;

    public PricingController(PricingService pricingService)
    {
        _pricingService = pricingService;
    }

    [HttpPost("estimate")]
    [AllowAnonymous]
    public IActionResult GetEstimate([FromBody] PricingRequest request)
    {
        var result = _pricingService.CalculateEstimate(request);
        return Ok(new { estimate = result });
    }
}
