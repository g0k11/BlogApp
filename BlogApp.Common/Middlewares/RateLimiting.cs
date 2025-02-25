using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace BlogApp.Common.Middlewares
{
    public class RateLimitingMiddleware(RequestDelegate _next, IMemoryCache _cache, int _requestLimit, TimeSpan _timeWindow)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var clientIp = GetClientIp(context);

            if (string.IsNullOrEmpty(clientIp))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Unable to determine client IP.");
                return;
            }
            var cacheKey = $"RateLimit_{clientIp}";
            if (!_cache.TryGetValue(cacheKey, out RateLimitInfo? rateLimitInfo))
            {
                rateLimitInfo = new RateLimitInfo { RequestCount = 0, StartTime = DateTime.UtcNow };
                _cache.Set(cacheKey, rateLimitInfo, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _timeWindow
                });
            }
            if (rateLimitInfo is not null && rateLimitInfo.RequestCount >= _requestLimit)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.Response.Headers["Retry-After"] = (_timeWindow - (DateTime.UtcNow - rateLimitInfo.StartTime)).TotalSeconds.ToString("F0");
                await context.Response.WriteAsync("Too many requests. Please try again later.");
                return;
            }
            if (rateLimitInfo is not null)
            {
                rateLimitInfo.RequestCount++;
                _cache.Set(cacheKey, rateLimitInfo, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _timeWindow
                });
            }
            await _next(context);
        }
        private string? GetClientIp(HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString();
        }
        private class RateLimitInfo
        {
            public int RequestCount { get; set; }
            public DateTime StartTime { get; set; }
        }
    }
}
