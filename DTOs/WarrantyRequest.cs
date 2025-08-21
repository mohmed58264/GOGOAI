using System;

public class WarrantyRequest
{
    public Guid OrderId { get; set; }
    public Guid ClientId { get; set; }
    public string ProblemDescription { get; set; }
}
