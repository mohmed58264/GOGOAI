
using System;

namespace FixoraBackend.Models
{
    public class BusinessContract
    {
        public int Id { get; set; }

        public int BusinessClientId { get; set; }

        public string ContractType { get; set; } // Annual, Quarterly, OnDemand

        public decimal? FixedAmountPerPeriod { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Status { get; set; } = "Active"; // Active, Paused, Cancelled, Completed

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public BusinessClient Client { get; set; }
    }
}

