
using System;
using System.Threading.Tasks;
using FixoraBackend.Helpers;
using FixoraBackend.Models;
using FixoraBackend.Data;
using Microsoft.EntityFrameworkCore;

namespace FixoraBackend.Services
{
    public class GeoAlertService
    {
        private readonly AppDbContext _context;
        private readonly NotificationService _notificationService;

        public GeoAlertService(AppDbContext context, NotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task CheckProviderProximityAsync(int orderId, double providerLat, double providerLng)
        {
            var order = await _context.Orders.Include(o => o.Client).FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null || order.Status != "InProgress")
                return;

            double distance = GeoHelper.DistanceInKm(order.Latitude, order.Longitude, providerLat, providerLng);

            // إذا المزود خرج من نطاق 0.5 كم
            if (distance > 0.5)
            {
                string message = $"تنبيه: المزود {order.ProviderId} خرج من نطاق موقع العميل للطلب رقم {order.Id}";
                await _notificationService.SendAdminAlertAsync(message);
            }
        }
    }
}
