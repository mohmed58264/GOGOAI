
using System;

namespace FixoraBackend.Models
{
    public class InvoicePayment
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public decimal AmountPaid { get; set; }

        public string PaymentMethod { get; set; } // Wallet, STCPay, BankTransfer, etc.

        public DateTime PaidAt { get; set; } = DateTime.UtcNow;

        public string PaidByUserId { get; set; }

        public Invoice Invoice { get; set; }
    }
}
