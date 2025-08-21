using System;

public class Wallet
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string System { get; set; }
    public double Balance { get; set; } = 0;
    public bool IsDeleted { get; set; } = false;
}
