
using System;

namespace FixoraBackend.Models
{
    public class PayoutRequest
    {
        public int Id { get; set; }

        public string UserId { get; set; } // صاحب المحفظة

        public decimal Amount { get; set; } // المبلغ المطلوب سحبه

        public string PaymentMethod { get; set; } // STCPay, BankTransfer

        public string PaymentDetails { get; set; } // رقم حساب أو رقم STCPay

        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Paid

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? PaidAt { get; set; }
    }
}

