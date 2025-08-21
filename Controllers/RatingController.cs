using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class RatingController : ControllerBase
{
    private readonly RatingService _service;

    public RatingController(RatingService service)
    {
        _service = service;
    }

    [HttpPost("submit")]
    [Authorize]
    public async Task<IActionResult> Submit([FromBody] RatingRequest request)
    {
        await _service.SubmitRatingAsync(request);
        return Ok(new { success = true });
    }

    [HttpGet("average")]
    [Authorize]
    public async Task<IActionResult> GetAvg([FromQuery] Guid userId, [FromQuery] string role)
    {
        var avg = await _service.GetAverageRating(userId, role);
        return Ok(new { average = avg });
    }
}
