
using System;
using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.DTOs
{
    public class CreateSaaSInvoiceRequest
    {
        [Required]
        public int SaaSPartnerId { get; set; }

        [Required]
        public string ClientName { get; set; }

        [Required]
        public string ClientContact { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        public string Notes { get; set; }
    }
}

