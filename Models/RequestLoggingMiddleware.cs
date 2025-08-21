using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var watch = Stopwatch.StartNew();
        var ip = context.Connection.RemoteIpAddress?.ToString();
        var path = context.Request.Path;

        _logger.LogInformation($"Request from {ip} to {path}");
        await _next(context);
        watch.Stop();
        _logger.LogInformation($"Completed in {watch.ElapsedMilliseconds} ms");
    }
}
