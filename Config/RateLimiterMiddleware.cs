using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class RateLimiterMiddleware
{
    private static readonly ConcurrentDictionary<string, (DateTime timestamp, int count)> _requests = new();
    private readonly RequestDelegate _next;

    public RateLimiterMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var now = DateTime.UtcNow;

        if (_requests.TryGetValue(ip, out var data))
        {
            if ((now - data.timestamp).TotalSeconds < 60)
            {
                if (data.count >= 100)
                {
                    context.Response.StatusCode = 429;
                    await context.Response.WriteAsync("Too many requests. Try again later.");
                    return;
                }

                _requests[ip] = (data.timestamp, data.count + 1);
            }
            else
            {
                _requests[ip] = (now, 1);
            }
        }
        else
        {
            _requests[ip] = (now, 1);
        }

        await _next(context);
    }
}
