
using System;

namespace FixoraBackend.Models
{
    public class ProviderLocationHistory
    {
        public int Id { get; set; }

        public string ProviderId { get; set; } // FK to ApplicationUser

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

        public int? OrderId { get; set; } // اختياري: ربط الموقع بطلب معين
    }
}