using System;

public class WarrantyCase
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ClientId { get; set; }
    public string ProblemDescription { get; set; }
    public string Status { get; set; } = "submitted"; // submitted, provider_rejected, escalated, resolved
    public string ProviderResponse { get; set; }
    public string SupervisorNote { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
