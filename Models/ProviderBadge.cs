
using System;

namespace FixoraBackend.Models
{
    public class ProviderBadge
    {
        public int Id { get; set; }

        public string ProviderUserId { get; set; }

        public string BadgeName { get; set; } // FastResponder, TopRated, MostOrders

        public DateTime AwardedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpiryDate { get; set; } // مثلاً نهاية الأسبوع أو الشهر
    }
}
