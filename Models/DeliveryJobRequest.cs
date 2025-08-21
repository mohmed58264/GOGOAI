using System;

public class DeliveryJobRequest
{
    public Guid RequestedById { get; set; }
    public string SourceType { get; set; } // e.g. "store", "cookie"
    public Guid SourceId { get; set; }
    public string PickupLocation { get; set; }
    public string DeliveryAddress { get; set; }
}
