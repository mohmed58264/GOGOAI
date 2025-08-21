
using System;
using System.Collections.Generic;

namespace FixoraBackend.Models
{
    public class SaaSPartner
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string OwnerUserId { get; set; } // رابط لحساب الأدمن للشركة

        public string Country { get; set; }

        public string City { get; set; }

        public bool IsVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<SaaSProvider> Providers { get; set; }
    }
}

