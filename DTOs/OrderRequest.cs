using System;

public class OrderRequest
{
    public Guid UserId { get; set; }
    public string Type { get; set; }
    public double Price { get; set; }
}
