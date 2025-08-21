using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class RatingService
{
    private readonly MainDbContext _db;

    public RatingService(MainDbContext db)
    {
        _db = db;
    }

    public async Task SubmitRatingAsync(RatingRequest request)
    {
        if (request.Stars < 1 || request.Stars > 5)
            throw new ArgumentException("Invalid star value");

        var exists = await _db.Ratings.AnyAsync(r => r.OrderId == request.OrderId && r.RatedByUserId == request.RatedByUserId && r.Role == request.Role);
        if (exists)
            throw new InvalidOperationException("Already rated");

        var rating = new Rating
        {
            Id = Guid.NewGuid(),
            OrderId = request.OrderId,
            RatedUserId = request.RatedUserId,
            RatedByUserId = request.RatedByUserId,
            Role = request.Role,
            Stars = request.Stars,
            Comment = request.Comment
        };

        _db.Ratings.Add(rating);
        await _db.SaveChangesAsync();
    }

    public async Task<double> GetAverageRating(Guid userId, string role)
    {
        var ratings = await _db.Ratings.Where(r => r.RatedUserId == userId && r.Role == role).ToListAsync();
        if (ratings.Count == 0) return 0;
        return ratings.Average(r => r.Stars);
    }
}
