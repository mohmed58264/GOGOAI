using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class DriverDeliveryService
{
    private readonly MainDbContext _db;

    public DriverDeliveryService(MainDbContext db)
    {
        _db = db;
    }

    public async Task<DeliveryJob> CreateJobAsync(DeliveryJobRequest req)
    {
        var job = new DeliveryJob
        {
            Id = Guid.NewGuid(),
            RequestedById = req.RequestedById,
            SourceType = req.SourceType,
            SourceId = req.SourceId,
            PickupLocation = req.PickupLocation,
            DeliveryAddress = req.DeliveryAddress
        };

        _db.DeliveryJobs.Add(job);
        await _db.SaveChangesAsync();
        return job;
    }

    public async Task AssignDriverAsync(Guid jobId, Guid driverId)
    {
        var job = await _db.DeliveryJobs.FindAsync(jobId);
        if (job == null) throw new Exception("Job not found");

        job.DriverId = driverId;
        job.Status = "assigned";

        await _db.SaveChangesAsync();
    }

    public async Task MarkPickedUpAsync(Guid jobId)
    {
        var job = await _db.DeliveryJobs.FindAsync(jobId);
        if (job == null) throw new Exception("Job not found");

        job.Status = "picked_up";
        await _db.SaveChangesAsync();
    }

    public async Task MarkDeliveredAsync(Guid jobId)
    {
        var job = await _db.DeliveryJobs.FindAsync(jobId);
        if (job == null) throw new Exception("Job not found");

        job.Status = "delivered";
        await _db.SaveChangesAsync();
    }

    public async Task<List<DeliveryJob>> GetDriverJobs(Guid driverId)
    {
        return await _db.DeliveryJobs
            .Where(j => j.DriverId == driverId)
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();
    }
}
