
using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.DTOs
{
    public class CreateSaaSProviderRequest
    {
        [Required]
        public string UserId { get; set; } // معرف المستخدم في النظام الرئيسي

        [Required]
        public int SaaSPartnerId { get; set; }

        [Required]
        public string Specialty { get; set; } // مجال العمل: سباكة، كهرباء، إلخ
    }
}

