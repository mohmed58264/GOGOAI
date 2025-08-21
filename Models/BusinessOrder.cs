using System;

public class BusinessOrder
{
    public Guid Id { get; set; }
    public Guid BusinessClientId { get; set; }
    public Guid BusinessSiteId { get; set; }
    public string ServiceType { get; set; }
    public double Price { get; set; }
    public string Status { get; set; } = "pending"; // pending, completed, invoiced
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
