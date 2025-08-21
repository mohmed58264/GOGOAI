using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.Models
{
    public class BusinessSiteRequest
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid BusinessClientId { get; set; }

        [Required]
        [MaxLength(200)]
        public string SiteName { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string ContactPhone { get; set; }

        // علاقة اختيارية: إذا كان لديك كيان BusinessClient
        // public virtual BusinessClient BusinessClient { get; set; }

    }
}
