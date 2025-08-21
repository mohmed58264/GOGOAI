
using System;
using System.Threading.Tasks;
using FixoraBackend.Data;
using FixoraBackend.Models;

namespace FixoraBackend.Services
{
    public class LocationHistoryService
    {
        private readonly AppDbContext _context;

        public LocationHistoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveProviderLocationAsync(string providerId, double latitude, double longitude, int? orderId = null)
        {
            var history = new ProviderLocationHistory
            {
                ProviderId = providerId,
                Latitude = latitude,
                Longitude = longitude,
                RecordedAt = DateTime.UtcNow,
                OrderId = orderId
            };

            _context.ProviderLocationHistories.Add(history);
            await _context.SaveChangesAsync();
        }
    }
}