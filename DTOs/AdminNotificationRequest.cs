
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FixoraBackend.DTOs
{
    public class AdminNotificationRequest
    {
        [Required]
        public string Title { get; set; } // عنوان الإشعار

        [Required]
        public string Message { get; set; } // نص الإشعار

        public List<string> UserIds { get; set; } // قائمة معرفات المستخدمين

        public string Role { get; set; } // إذا أراد الأدمن إرسال الإشعار لكل دور معين (Client, Provider...)
    }
}
