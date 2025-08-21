
using System;

namespace FixoraBackend.Models
{
    public class UserReferral
    {
        public int Id { get; set; }

        public string ReferrerUserId { get; set; }

        public string ReferredUserId { get; set; }

        public DateTime ReferredAt { get; set; } = DateTime.UtcNow;

        public bool IsVerified { get; set; } = false;
    }
}

