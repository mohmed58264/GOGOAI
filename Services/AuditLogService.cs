using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class AuditLogService
{
    private readonly MainDbContext _db;

    public AuditLogService(MainDbContext db)
    {
        _db = db;
    }

    public async Task LogAsync(Guid userId, string actionType, string entity, string description)
    {
        var log = new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ActionType = actionType,
            Entity = entity,
            Description = description,
            Timestamp = DateTime.UtcNow
        };

        _db.AuditLogs.Add(log);
        await _db.SaveChangesAsync();
    }

    public async Task<List<AuditLog>> GetLogsAsync(int page = 1, int pageSize = 20)
    {
        return await _db.AuditLogs
            .AsNoTracking()
            .OrderByDescending(x => x.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
