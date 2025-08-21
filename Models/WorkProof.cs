
using System;

namespace FixoraBackend.Models
{
    public class WorkProof
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public string ProviderId { get; set; }

        public string FileUrl { get; set; } // رابط الصورة أو الفيديو
        public string FileType { get; set; } // image/jpeg, video/mp4

        public string Description { get; set; } // ملاحظات اختيارية

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
