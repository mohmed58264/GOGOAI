using System;

public class RatingRequest
{
    public Guid OrderId { get; set; }
    public Guid RatedUserId { get; set; }
    public Guid RatedByUserId { get; set; }
    public string Role { get; set; }
    public int Stars { get; set; }
    public string Comment { get; set; }
}
