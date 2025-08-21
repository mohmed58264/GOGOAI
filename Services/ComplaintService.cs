using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class ComplaintService
{
    private readonly MainDbContext _db;

    public ComplaintService(MainDbContext db)
    {
        _db = db;
    }

    public async Task<Complaint> SubmitComplaintAsync(ComplaintRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            throw new ArgumentException("Message is required");

        var orderExists = await _db.Orders.AnyAsync(o => o.Id == request.OrderId);
        if (!orderExists)
            throw new InvalidOperationException("Order not found");

        var complaint = new Complaint
        {
            Id = Guid.NewGuid(),
            OrderId = request.OrderId,
            UserId = request.UserId,
            Message = request.Message,
            Status = "submitted",
            CreatedAt = DateTime.UtcNow
        };

        _db.Complaints.Add(complaint);
        await _db.SaveChangesAsync();

        return complaint;
    }
}
