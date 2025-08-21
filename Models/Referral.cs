using System;

namespace FixoraBackend.Models
{
    public class Referral
    {
        public int Id { get; set; }

        public string ReferrerUserId { get; set; } // من قام بالإحالة

        public string ReferredUserId { get; set; } // المستخدم الذي تم إحالته

        public DateTime ReferredAt { get; set; } = DateTime.UtcNow;

        public decimal CommissionEarned { get; set; } = 0m; // العمولة المكتسبة

        public bool IsCommissionPaid { get; set; } = false;
    }
}
