
using System;

namespace FixoraBackend.Models
{
    public class ReferralEarning
    {
        public int Id { get; set; }

        public string ReferrerUserId { get; set; }

        public int RelatedOrderId { get; set; }

        public decimal Amount { get; set; }

        public bool IsWithdrawn { get; set; } = false;

        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;
    }
}
