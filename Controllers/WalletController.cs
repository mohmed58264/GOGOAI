using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly WalletService _walletService;

    public WalletController(WalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet("{userId}/{system}")]
    [Authorize]
    public async Task<IActionResult> GetWallet(Guid userId, string system)
    {
        var wallet = await _walletService.GetWalletAsync(userId, system);
        return Ok(wallet);
    }

    [HttpPost("add")]
    [Authorize]
    public async Task<IActionResult> AddTransaction([FromBody] AddTransactionRequest request)
    {
        await _walletService.AddTransactionAsync(request.UserId, request.System, request.Amount, request.Type);
        return Ok(new { success = true });
    }
}
