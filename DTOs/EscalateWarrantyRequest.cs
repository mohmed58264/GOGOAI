
using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.DTOs
{
    public class EscalateWarrantyRequest
    {
        [Required]
        public int WarrantyId { get; set; }

        [Required]
        public string EscalationLevel { get; set; } // Provider, Supervisor, Admin

        [Required]
        public string Reason { get; set; }
    }
}

