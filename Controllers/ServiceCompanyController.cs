using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixoraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceCompanyController : ControllerBase
    {
        private readonly IServiceCompanyService _service;

        public ServiceCompanyController(IServiceCompanyService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        [Authorize(Roles = "Provider,Admin")]
        public async Task<IActionResult> RegisterCompany([FromBody] CompanyRegistrationRequest request)
        {
            var userId = User.FindFirst("uid")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found.");

            var result = await _service.RegisterCompanyAsync(request, userId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCompany(int id)
        {
            var company = await _service.GetCompanyByIdAsync(id);
            if (company == null)
                return NotFound("Company not found.");

            return Ok(company);
        }

        [HttpPost("assign-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignUser([FromQuery] string userId, [FromQuery] int companyId, [FromQuery] string role)
        {
            var success = await _service.AssignUserToCompanyAsync(userId, companyId, role);
            if (!success)
                return BadRequest("User already assigned or failed.");

            return Ok("User assigned successfully.");
        }
    }
}
