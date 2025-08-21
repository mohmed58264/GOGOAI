
using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.DTOs
{
    public class CreateReferralRequest
    {
        [Required]
        public string ReferrerUserId { get; set; } // من قام بالإحالة
        [Required]
        public string ReferredUserId { get; set; } // من تمّت إحالته
    }
}
