
using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.DTOs
{
    public class CreateInvoicePaymentRequest
    {
        [Required]
        public int InvoiceId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } // Wallet, STCPay, BankTransfer, etc.
    }
}

