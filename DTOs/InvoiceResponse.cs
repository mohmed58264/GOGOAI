namespace FixoraBackend.DTOs
{
    public class InvoiceResponse
    {
        public int InvoiceId { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public int? InitialPaymentPercentage { get; set; } // مثال: 50 تعني 50%
        public int WarrantyDays { get; set; }
        public string Status { get; set; } = string.Empty; // Pending, Approved, Rejected
        public string CreatedAt { get; set; } = string.Empty;
        public string? ProviderNote { get; set; }
        public string? CustomerNote { get; set; }
    }
}

