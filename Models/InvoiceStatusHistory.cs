
using System;

namespace FixoraBackend.Models
{
    public class InvoiceStatusHistory
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public string Status { get; set; } // Pending, Approved, Rejected, AwaitingFirstPayment...

        public string ChangedByUserId { get; set; }

        public string Note { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        public Invoice Invoice { get; set; }
    }
}
