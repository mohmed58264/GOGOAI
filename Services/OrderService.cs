using FixoraBackend.Interfaces;
using FixoraBackend.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static OrderService;

public class OrderService
{

    private readonly AppDbContext _context;
    private readonly IReferralService _referralService;

    public OrderService(AppDbContext context, IReferralService referralService)
    {
        _context = context;
        _referralService = referralService;
    }


    public async Task<Order> CreateOrderAsync(Guid userId, string type, double price)
    {
        if (price <= 0)
            throw new ArgumentException("Price must be greater than 0");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = type,
            Price = price,
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return order;
    }

    public async Task<bool> CompleteOrderAsync(Guid orderId)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
            throw new InvalidOperationException("Order not found");

        order.Status = "completed";
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CompleteOrderAsync(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null) return false;

        order.Status = OrderStatus.Completed;
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();

        // ✅ إضافة العمولة للإحالة إذا كان العميل قد تمّت إحالته
        var referral = await _context.Referrals
            .FirstOrDefaultAsync(r => r.ReferredUserId == order.ClientUserId && !r.IsCommissionPaid);

        if (referral != null)
        {
            decimal commissionAmount = order.TotalAmount * 0.05m; // 5% عمولة الإحالة
            await _referralService.AddCommissionAsync(referral.Id, commissionAmount);
        }

        return true;
    }
}