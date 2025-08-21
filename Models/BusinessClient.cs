using System;

public class BusinessClient
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; }
    public string ManagerName { get; set; }
    public string Phone { get; set; }
    public string Region { get; set; }
    public string PaymentType { get; set; } // "per_order", "monthly", "semi_monthly"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

