
using System;

namespace FixoraBackend.Models
{
    public class SaaSProvider
    {
        public int Id { get; set; }

        public string UserId { get; set; } // معرف المستخدم في النظام الرئيسي

        public int SaaSPartnerId { get; set; }

        public string Specialty { get; set; } // مجال العمل: سباكة، كهرباء، إلخ

        public bool IsActive { get; set; } = true;

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public SaaSPartner Partner { get; set; }
    }
}

