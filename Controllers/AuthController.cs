using FixoraBackend.Models;
using FixoraBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _authService.LoginAsync(request);
        return Ok(new { token });
    }

    
if (model.Role == "Supervisor")
{
    await _supervisorService.CreateSupervisorAsync(user.Id);
}
else if (model.Role == "Provider" && !string.IsNullOrEmpty(model.SupervisorCode))
{
    var provider = new Provider
    {
        Name = model.FullName,
        IsActive = false,
        Specializations = "",
        // باقي الحقول...
    };

    await _supervisorService.RegisterProviderAsync(provider, model.SupervisorCode);
}
}



        [HttpPost("change-provider-supervisor")]
public async Task<IActionResult> ChangeProviderSupervisor(int providerId, int newSupervisorId)
{
    var success = await _supervisorService.ChangeProviderSupervisorAsync(providerId, newSupervisorId, "Admin");

    if (!success)
        return BadRequest(new { message = "Failed to change supervisor" });

    return Ok(new { message = "Supervisor changed successfully" });
}
    }}









