using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
    {
        var result = await _orderService.CreateOrderAsync(request.UserId, request.Type, request.Price);
        return Ok(result);
    }

    [HttpPost("complete/{orderId}")]
    [Authorize(Roles = "admin,provider")]
    public async Task<IActionResult> CompleteOrder(Guid orderId)
    {
        await _orderService.CompleteOrderAsync(orderId);
        return Ok(new { success = true });
    }



    [Authorize(Roles = "Provider")]
    [HttpPost("{orderId}/confirm-arrival")]
    public async Task<IActionResult> ConfirmArrival(int orderId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.ProviderId == userId);

        if (order == null)
            return NotFound(new { message = "Order not found or not assigned to this provider" });

        if (order.IsArrivalConfirmed)
            return BadRequest(new { message = "Arrival already confirmed" });

        order.IsArrivalConfirmed = true;
        order.ArrivalConfirmedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // ≈—”«· ≈‘⁄«— ··⁄„Ì·
        await _notificationService.SendOrderUpdateAsync(order.ClientId, order.Id, "„“Êœ «·Œœ„… Ê’· ·„Êﬁ⁄ﬂ Ê»œ√ «·⁄„·");

        return Ok(new { message = "Arrival confirmed successfully" });
    }



    [Authorize(Roles = "Provider")]
    [HttpPost("{orderId}/complete-work")]
    public async Task<IActionResult> CompleteWorkByProvider(int orderId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.ProviderId == userId);

        if (order == null)
            return NotFound(new { message = "Order not found or not assigned to this provider" });

        if (order.IsWorkCompletedByProvider)
            return BadRequest(new { message = "Work already marked as completed" });

        //  Õﬁﬁ √‰ ﬂ· «·œ›⁄«   „ 
        var totalPaid = await _context.Payments
            .Where(p => p.OrderId == orderId && p.Status == "Paid")
            .SumAsync(p => p.Amount);

        if (totalPaid < order.TotalAmount)
            return BadRequest(new { message = "All payments must be completed before marking work as finished" });

        order.IsWorkCompletedByProvider = true;
        order.WorkCompletedAt = DateTime.UtcNow;
        order.Status = "PendingClientConfirmation";

        await _context.SaveChangesAsync();

        // ≈‘⁄«— «·⁄„Ì·
        await _notificationService.SendOrderUpdateAsync(order.ClientId, order.Id, "„“Êœ «·Œœ„… √ﬂœ ≈‰Â«¡ «·⁄„·. Ì—ÃÏ  √ﬂÌœ «·«” ·«„.");

        return Ok(new { message = "Work marked as completed. Waiting for client confirmation." });
    }



    [Authorize(Roles = "Client")]
    [HttpPost("{orderId}/confirm-receipt")]
    public async Task<IActionResult> ConfirmReceiptByClient(int orderId, [FromBody] string note)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.ClientId == userId);

        if (order == null)
            return NotFound(new { message = "Order not found or not assigned to this client" });

        if (!order.IsWorkCompletedByProvider)
            return BadRequest(new { message = "Provider has not marked work as completed yet" });

        if (order.IsWorkConfirmedByClient)
            return BadRequest(new { message = "Work already confirmed by client" });

        order.IsWorkConfirmedByClient = true;
        order.WorkConfirmedAt = DateTime.UtcNow;
        order.ClientCompletionNote = note;
        order.Status = "Completed";

        await _context.SaveChangesAsync();

        // ≈ÿ·«ﬁ «·œ›⁄ «·‰Â«∆Ì ··„“Êœ
        await _paymentService.ReleasePaymentToProvider(order);

        return Ok(new { message = "Work confirmed successfully. Order closed." });
    }



    [Authorize(Roles = "Client")]
    [HttpPost("{orderId}/reject-completion")]
    public async Task<IActionResult> RejectCompletion(int orderId, [FromBody] string reason)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.ClientId == userId);

        if (order == null)
            return NotFound(new { message = "Order not found or not assigned to this client" });

        if (!order.IsWorkCompletedByProvider)
            return BadRequest(new { message = "Provider has not marked work as completed yet" });

        // › Õ ÷„«‰  ·ﬁ«∆Ì
        var warrantyCase = new WarrantyCase
        {
            OrderId = orderId,
            ClientId = userId,
            Description = reason,
            Status = "Open",
            CreatedAt = DateTime.UtcNow
        };

        _context.WarrantyCases.Add(warrantyCase);
        order.Status = "WarrantyOpened";

        await _context.SaveChangesAsync();

        return Ok(new { message = "Completion rejected. Warranty case opened." });
    }


    [Authorize(Roles = "Client")]
    [HttpPost("{orderId}/reject-completion")]
    public async Task<IActionResult> RejectCompletion(int orderId, [FromBody] string reason)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.ClientId == userId);

        if (order == null)
            return NotFound(new { message = "Order not found or not assigned to this client" });

        if (!order.IsWorkCompletedByProvider)
            return BadRequest(new { message = "Provider has not marked work as completed yet" });

        order.Status = "PendingSupervisorReview";
        order.ClientCompletionNote = reason;

        await _context.SaveChangesAsync();

        // ≈‘⁄«— «·„“Êœ Ê«·≈œ«—…
        await _notificationService.SendOrderUpdateAsync(order.ProviderId, order.Id, "«·⁄„Ì· —›÷ ≈ „«„ «·ÿ·». Ì—ÃÏ „—«Ã⁄… «·”Ê»—›«Ì“—.");
        await _notificationService.SendOrderUpdateAsync("ADMIN", order.Id, "ÿ·» »«‰ Ÿ«— „—«Ã⁄… «·”Ê»—›«Ì“—.");

        return Ok(new { message = "Completion rejected. Supervisor will review." });
    }




    [Authorize(Roles = "Provider")]
    [HttpPost("{orderId}/escalate")]
    public async Task<IActionResult> EscalateToSupervisor(int orderId, [FromBody] string note)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.ProviderId == userId);

        if (order == null)
            return NotFound(new { message = "Order not found or not assigned to this provider" });

        if (order.Status != "PendingSupervisorReview")
            return BadRequest(new { message = "Order is not pending supervisor review" });

        order.Status = "EscalatedToSupervisor";
        order.ProviderNote = note;

        await _context.SaveChangesAsync();

        // ≈‘⁄«— «·”Ê»—›«Ì“—
        await _notificationService.SendOrderUpdateAsync("SUPERVISOR", order.Id, " „  ’⁄Ìœ «·ÿ·» „‰ «·„“Êœ ··„—«Ã⁄….");

        return Ok(new { message = "Order escalated to supervisor." });
    }

}
