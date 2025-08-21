using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FixoraBackend.DTOs;
using FixoraBackend.Interfaces;

namespace FixoraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessContractController : ControllerBase
    {
        private readonly IBusinessContractService _contractService;

        public BusinessContractController(IBusinessContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin,BusinessClient")]
        public async Task<IActionResult> CreateContract([FromBody] CreateBusinessContractRequest request)
        {
            var contract = await _contractService.CreateContractAsync(request);
            return Ok(contract);
        }

        [HttpGet("by-client/{clientId}")]
        [Authorize(Roles = "Admin,BusinessClient")]
        public async Task<IActionResult> GetContractByClient(int clientId)
        {
            var contract = await _contractService.GetContractByClientIdAsync(clientId);
            if (contract == null)
                return NotFound();

            return Ok(contract);
        }
    }
}

