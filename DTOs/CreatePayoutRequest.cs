
using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.DTOs
{
    public class CreatePayoutRequest
    {
        [Required]
        public decimal Amount { get; set; } // المبلغ المطلوب سحبه

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } // STCPay, BankTransfer

        [Required]
        [StringLength(200)]
        public string PaymentDetails { get; set; } // رقم الحساب أو رقم STCPay
    }
}

