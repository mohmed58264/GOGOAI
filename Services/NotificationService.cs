using FixoraBackend.Data;
using FixoraBackend.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class NotificationService
{
    private readonly MainDbContext _db;

    public NotificationService(MainDbContext db)
    {
        _db = db;
    }

    public async Task<List<Notification>> GetUserNotifications(Guid userId)
    {
        return await _db.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task SendNotification(Guid userId, string message)
    {
        var notif = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Message = message
        };
        _db.Notifications.Add(notif);
        await _db.SaveChangesAsync();
    }

    public async Task MarkAsRead(Guid notificationId)
    {
        var notif = await _db.Notifications.FindAsync(notificationId);
        if (notif != null)
        {
            notif.IsRead = true;
            await _db.SaveChangesAsync();
        }
    }

    public async Task SendPayoutNotificationAsync(string userId, string status, decimal amount)
    {
        string message = status switch
        {
            "Paid" => $"✅ تم دفع طلب السحب الخاص بك بمبلغ {amount} ريال.",
            "Rejected" => $"❌ تم رفض طلب السحب الخاص بك بمبلغ {amount} ريال.",
            _ => $"📢 تم تحديث حالة طلب السحب الخاص بك."
        };

        // 🔹 هنا يمكن إضافة منطق الإرسال الفعلي:
        // 1. إرسال Push Notification عبر Firebase
        // 2. إرسال بريد إلكتروني
        // 3. إرسال SMS

        Console.WriteLine($"[Notification] To: {userId} - {message}");
        await Task.CompletedTask;
    }

    public async Task SendAdminAlertAsync(string message)
    {
        var admins = await _context.Users.Where(u => u.Role == "Admin").ToListAsync();
        foreach (var admin in admins)
        {
            // هنا ممكن يكون إرسال إشعار Push أو Email
            Console.WriteLine($"[ADMIN ALERT] {message}");
        }
    }





}
