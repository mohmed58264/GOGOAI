using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly NotificationService _service;

    public NotificationController(NotificationService service)
    {
        _service = service;
    }

    [HttpGet("{userId}")]
    [Authorize]
    public async Task<IActionResult> Get(Guid userId)
    {
        var result = await _service.GetUserNotifications(userId);
        return Ok(result);
    }

    [HttpPost("read/{id}")]
    [Authorize]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        await _service.MarkAsRead(id);
        return Ok(new { success = true });
    }
}
