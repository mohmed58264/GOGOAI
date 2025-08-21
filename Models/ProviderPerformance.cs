using System;

public class ProviderPerformance
{
    public Guid Id { get; set; }
    public Guid ProviderId { get; set; }
    public int CompletedOrders { get; set; }
    public double AverageRating { get; set; }
    public int WeekNumber { get; set; }
    public int Year { get; set; }
    public string Rank { get; set; } // "normal", "pro", "elite"
    public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
}

