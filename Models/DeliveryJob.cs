using System;

public class DeliveryJob
{
    public Guid Id { get; set; }
    public Guid RequestedById { get; set; }
    public string SourceType { get; set; } // "store", "cookie", "custom"
    public Guid SourceId { get; set; }
    public Guid? DriverId { get; set; }
    public string Status { get; set; } = "pending"; // pending, assigned, picked_up, delivered
    public string PickupLocation { get; set; }
    public string DeliveryAddress { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
