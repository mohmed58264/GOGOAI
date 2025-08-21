using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.DTOs
{
    public class CompanyRegistrationRequest
    {
        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string CommercialNumber { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
