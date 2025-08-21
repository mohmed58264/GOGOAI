
using System;

namespace FixoraBackend.Models
{
    public class WarrantyEscalation
    {
        public int Id { get; set; }

        public int WarrantyId { get; set; }

        public string EscalatedByUserId { get; set; }

        public string EscalationLevel { get; set; } // Provider, Supervisor, Admin

        public string Reason { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Resolved, Rejected

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ResolvedAt { get; set; }

        public Warranty Warranty { get; set; }
    }
}

