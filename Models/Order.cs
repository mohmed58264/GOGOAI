using System;

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Type { get; set; }
    public double Price { get; set; }
    public string Status { get; set; } = "pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsArrivalConfirmed { get; set; } = false;
    public DateTime? ArrivalConfirmedAt { get; set; }

    public bool IsWorkCompletedByProvider { get; set; } = false; // ÃßÏ ÇáãÒæÏ ÅäåÇÁ ÇáÚãá
    public DateTime? WorkCompletedAt { get; set; }

    public bool IsWorkConfirmedByClient { get; set; } = false; // ÃßÏ ÇáÚãíá ÇáÇÓÊáÇã
    public DateTime? WorkConfirmedAt { get; set; }

    public string ClientCompletionNote { get; set; } // ãáÇÍÙÇÊ ÇáÚãíá ÚäÏ ÇáÑİÖ Ãæ ÇáãæÇİŞÉ


}
