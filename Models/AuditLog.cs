using System;

public class AuditLog
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string ActionType { get; set; }
    public string Entity { get; set; }
    public string Description { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

