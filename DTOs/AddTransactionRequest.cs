using System;

public class AddTransactionRequest
{
    public Guid UserId { get; set; }
    public string System { get; set; }
    public double Amount { get; set; }
    public string Type { get; set; }
}
