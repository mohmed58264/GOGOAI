using System;

namespace FixoraBackend.Models
{
    public class CompanyUser
    {
        public int Id { get; set; }

        public string UserId { get; set; } // FK to ApplicationUser

        public int ServiceCompanyId { get; set; } // FK to ServiceCompany

        public string Role { get; set; } // Optional: Admin, Staff, Viewer...

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        public ServiceCompany Company { get; set; }
    }
}

