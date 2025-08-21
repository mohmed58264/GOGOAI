using System;

public class Complaint
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public string Message { get; set; }
    public string Status { get; set; } = "submitted";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
