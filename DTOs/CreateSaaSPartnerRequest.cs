
using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.DTOs
{
    public class CreateSaaSPartnerRequest
    {
        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string OwnerUserId { get; set; } // معرف الأدمن للشركة

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }
    }
}

