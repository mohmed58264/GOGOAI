
using System;

namespace FixoraBackend.Models
{
    public class InvoiceInstallment
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime DueDate { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}

