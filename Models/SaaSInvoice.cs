
using System;

namespace FixoraBackend.Models
{
    public class SaaSInvoice
    {
        public int Id { get; set; }

        public int SaaSPartnerId { get; set; }

        public string ClientName { get; set; }

        public string ClientContact { get; set; }

        public decimal Amount { get; set; }

        public bool IsPaid { get; set; } = false;

        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        public DateTime? PaidAt { get; set; }

        public string Notes { get; set; }

        public SaaSPartner Partner { get; set; }
    }
}

