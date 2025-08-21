
using System;

namespace FixoraBackend.Models
{
    public class Provider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = false;
        public string Specializations { get; set; } // مهن مزود الخدمة

        public int? SupervisorId { get; set; }
        public Supervisor Supervisor { get; set; }
    }
}