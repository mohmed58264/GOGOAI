using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class SettingService
{
    private readonly MainDbContext _db;

    public SettingService(MainDbContext db)
    {
        _db = db;
    }

    public async Task<string> GetSettingAsync(string key)
    {
        var setting = await _db.Settings.FirstOrDefaultAsync(s => s.Key == key);
        return setting?.Value;
    }

    public async Task SaveSettingAsync(string key, string value)
    {
        var setting = await _db.Settings.FirstOrDefaultAsync(s => s.Key == key);
        if (setting == null)
        {
            _db.Settings.Add(new SystemSetting { Id = Guid.NewGuid(), Key = key, Value = value });
        }
        else
        {
            setting.Value = value;
        }

        await _db.SaveChangesAsync();
    }
}
