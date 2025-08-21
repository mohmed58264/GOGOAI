
using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.DTOs
{
    public class ApproveInvoiceRequest
    {
        [Required]
        public int InvoiceId { get; set; }

        public string? CustomerNote { get; set; }

        public bool Approve { get; set; }
    }
}
