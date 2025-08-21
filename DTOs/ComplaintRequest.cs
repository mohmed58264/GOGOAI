using System;

public class ComplaintRequest
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public string Message { get; set; }
}
