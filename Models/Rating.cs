using System;

public class Rating
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid RatedByUserId { get; set; }
    public Guid RatedUserId { get; set; }
    public string Role { get; set; } // "provider" or "driver"
    public int Stars { get; set; } // 1 to 5
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
