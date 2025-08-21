using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.DTOs.Requests
{
    public class BusinessSiteRequestDto
    {
        [Required]
        public Guid BusinessClientId { get; set; }

        [Required, MaxLength(100)]
        public string SiteName { get; set; }

        [Required, MaxLength(200)]
        public string Address { get; set; }

        [Required, MaxLength(20)]
        public string ContactPhone { get; set; }
    }
}
