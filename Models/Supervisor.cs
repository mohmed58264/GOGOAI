
using System;
using System.Collections.Generic;

namespace FixoraBackend.Models
{
    public class Supervisor
    {
        public int Id { get; set; }
        public string UserId { get; set; } // ربط بحساب المستخدم
        public string Code { get; set; } // كود فريد من 3 أرقام
        public decimal CommissionRate { get; set; } = 0.05M; // نسبة العمولة
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Provider> Providers { get; set; }
    }
}
