using FixoraBackend.DTOs.DTOs;
using FixoraBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class BusinessController : ControllerBase
{
    private readonly BusinessService _service;

    public BusinessController(BusinessService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Register([FromBody] BusinessClientRequest request)
    {
        var result = await _service.CreateClientAsync(request);
        return Ok(result);
    }

    [HttpPost("add-site")]
    [Authorize(Roles = "admin,business")]
    public async Task<IActionResult> AddSite([FromBody] BusinessSiteRequest request)
    {
        var site = await _service.AddSiteAsync(request);
        return Ok(site);
    }

    [HttpPost("create-order")]
    [Authorize(Roles = "business")]
    public async Task<IActionResult> CreateOrder([FromBody] BusinessOrderRequest request)
    {
        var order = await _service.CreateOrderAsync(request);
        return Ok(order);
    }

    [HttpGet("{clientId}/orders")]
    [Authorize(Roles = "business,admin")]
    public async Task<IActionResult> GetOrders(Guid clientId)
    {
        var orders = await _service.GetClientOrders(clientId);
        return Ok(orders);
    }
}
