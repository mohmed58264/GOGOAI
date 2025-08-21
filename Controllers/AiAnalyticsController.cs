using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AiAnalyticsController : ControllerBase
{
    private readonly AiAnalysisService _aiService;

    public AiAnalyticsController(AiAnalysisService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("rank-weekly")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> RunWeeklyRank()
    {
        await _aiService.CalculateWeeklyRanks();
        return Ok(new { success = true });
    }

    [HttpGet("rank/{providerId}")]
    [Authorize(Roles = "provider,admin")]
    public async Task<IActionResult> GetCurrentRank(Guid providerId)
    {
        var rank = await _aiService.GetCurrentRank(providerId);
        return Ok(new { rank });
    }
}
