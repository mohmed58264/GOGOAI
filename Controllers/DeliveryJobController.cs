using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class DeliveryJobController : ControllerBase
{
    private readonly DriverDeliveryService _service;

    public DeliveryJobController(DriverDeliveryService service)
    {
        _service = service;
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateJob([FromBody] DeliveryJobRequest request)
    {
        var job = await _service.CreateJobAsync(request);
        return Ok(job);
    }

    [HttpPost("assign")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> AssignDriver([FromBody] AssignDriverToJob request)
    {
        await _service.AssignDriverAsync(request.DeliveryJobId, request.DriverId);
        return Ok(new { success = true });
    }

    [HttpPost("pickup/{id}")]
    [Authorize(Roles = "driver")]
    public async Task<IActionResult> MarkPickedUp(Guid id)
    {
        await _service.MarkPickedUpAsync(id);
        return Ok(new { success = true });
    }

    [HttpPost("deliver/{id}")]
    [Authorize(Roles = "driver")]
    public async Task<IActionResult> MarkDelivered(Guid id)
    {
        await _service.MarkDeliveredAsync(id);
        return Ok(new { success = true });
    }

    [HttpGet("my-jobs/{driverId}")]
    [Authorize(Roles = "driver")]
    public async Task<IActionResult> GetDriverJobs(Guid driverId)
    {
        var jobs = await _service.GetDriverJobs(driverId);
        return Ok(jobs);
    }
}
