using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class AiAnalysisService
{
    private readonly MainDbContext _db;

    public AiAnalysisService(MainDbContext db)
    {
        _db = db;
    }

    public async Task CalculateWeeklyRanks()
    {
        var start = DateTime.UtcNow.Date.AddDays(-7);
        var providers = await _db.Users
            .Where(u => u.Role == "provider")
            .ToListAsync();

        foreach (var provider in providers)
        {
            var orders = await _db.Orders
                .Where(o => o.UserId == provider.Id && o.Status == "completed" && o.CreatedAt >= start)
                .ToListAsync();

            var ratings = await _db.Ratings
                .Where(r => r.RatedUserId == provider.Id && r.Role == "provider" && r.CreatedAt >= start)
                .ToListAsync();

            var completed = orders.Count;
            var avgRating = ratings.Count > 0 ? ratings.Average(r => r.Stars) : 0;

            string rank = "normal";
            if (completed >= 10 && avgRating >= 4.5) rank = "pro";
            if (completed >= 20 && avgRating >= 4.8) rank = "elite";

            var perf = new ProviderPerformance
            {
                Id = Guid.NewGuid(),
                ProviderId = provider.Id,
                CompletedOrders = completed,
                AverageRating = avgRating,
                WeekNumber = ISOWeek.GetWeekOfYear(DateTime.UtcNow),
                Year = DateTime.UtcNow.Year,
                Rank = rank
            };

            _db.ProviderPerformances.Add(perf);
        }

        await _db.SaveChangesAsync();
    }

    public async Task<string> GetCurrentRank(Guid providerId)
    {
        var week = ISOWeek.GetWeekOfYear(DateTime.UtcNow);
        var year = DateTime.UtcNow.Year;

        var record = await _db.ProviderPerformances
            .Where(p => p.ProviderId == providerId && p.WeekNumber == week && p.Year == year)
            .OrderByDescending(p => p.CalculatedAt)
            .FirstOrDefaultAsync();

        return record?.Rank ?? "normal";
    }
}
