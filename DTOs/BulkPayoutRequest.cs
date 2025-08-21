
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.DTOs
{
    public class BulkPayoutRequest
    {
        [Required]
        public List<int> PayoutIds { get; set; } // معرفات طلبات السحب

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } // STCPay أو BankTransfer
    }
}