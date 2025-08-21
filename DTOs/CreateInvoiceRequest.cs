
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.DTOs
{
    public class CreateInvoiceRequest
    {
        [Required]
        public string ClientId { get; set; }

        public int? BusinessClientId { get; set; }

        public int? BusinessSiteId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        public bool RequiresFirstPayment { get; set; } = true;

        public decimal? FirstPaymentAmount { get; set; }

        public int InstallmentsCount { get; set; } = 1;

        public int WarrantyMonths { get; set; } = 0;

        public decimal AppCommissionRate { get; set; } = 0.1M;

        public DateTime? ExpiryDate { get; set; }

        public string? ProviderNote { get; set; }

        public List<InvoiceItemDTO> Items { get; set; }



        public decimal DownPayment { get; set; }
        public int WarrantyDays { get; set; }

        public List<InstallmentRequest> Installments { get; set; }





        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

    }

    public class InvoiceInstallmentDto
    {
        [Required]
        [Range(1, double.MaxValue)]
        public decimal Amount { get; set; }

        public DateTime? DueDate { get; set; }
    }
}

    public class InstallmentRequest
    {
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class InvoiceItemDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
    }
}