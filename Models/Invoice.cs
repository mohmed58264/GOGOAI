
using System;
using System.Collections.Generic;

namespace FixoraBackend.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        public string InvoiceNumber { get; set; }

        public string CreatedByUserId { get; set; }

        public string ClientId { get; set; } // End User (فرد أو أعمال)
        public int? BusinessClientId { get; set; } // If from company
        public int? BusinessSiteId { get; set; }

        public int OrderId { get; set; }

        public decimal TotalAmount { get; set; }

        public bool RequiresFirstPayment { get; set; } = true;

        public decimal? FirstPaymentAmount { get; set; }

        public bool FirstPaymentPaid { get; set; } = false;

        public int InstallmentsCount { get; set; } = 1;

        public int WarrantyMonths { get; set; } = 0;

        public decimal AppCommissionRate { get; set; } = 0.1M;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ExpiryDate { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, AwaitingFirstPayment, Approved, Rejected

        public string? CustomerNote { get; set; }

        public string? ProviderNote { get; set; }

        public bool IsDeleted { get; set; } = false;

        
            public string ProviderId { get; set; } // مزود الخدمة

            public decimal DownPayment { get; set; } // الدفعة الأولى
            public int WarrantyDays { get; set; } // مدة الضمان بالأيام




    public ICollection<InvoiceItem> Items { get; set; }

        public ICollection<InvoiceStatusHistory> StatusHistory { get; set; }
    }
}

