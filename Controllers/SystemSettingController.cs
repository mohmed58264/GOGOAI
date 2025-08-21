using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly SettingService _settingService;

    public SettingsController(SettingService settingService)
    {
        _settingService = settingService;
    }

    [HttpGet("{key}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetSetting(string key)
    {
        var value = await _settingService.GetSettingAsync(key);
        return Ok(new { key, value });
    }

    [HttpPost("save")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> SaveSetting([FromQuery] string key, [FromQuery] string value)
    {
        await _settingService.SaveSettingAsync(key, value);
        return Ok(new { success = true });
    }
}
