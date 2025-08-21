using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ComplaintController : ControllerBase
{
    private readonly ComplaintService _complaintService;

    public ComplaintController(ComplaintService complaintService)
    {
        _complaintService = complaintService;
    }

    [HttpPost("submit")]
    [Authorize]
    public async Task<IActionResult> Submit([FromBody] ComplaintRequest request)
    {
        var result = await _complaintService.SubmitComplaintAsync(request);
        return Ok(result);
    }
}
