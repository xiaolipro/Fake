using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Fake.AspNetCore.Http;

public class HttpClientInfoProvider(
    ILogger<HttpClientInfoProvider> logger,
    IHttpContextAccessor httpContextAccessor)
    : IHttpClientInfoProvider
{
    public string? UserAgent => GetUserAgent();
    public string? ClientIpAddress => GetClientIpAddress();

    protected virtual string? GetUserAgent()
    {
        return httpContextAccessor.HttpContext?.Request.Headers["User-Agent"];
    }

    protected virtual string? GetClientIpAddress()
    {
        try
        {
            if (httpContextAccessor.HttpContext?.Connection != null)
                return httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        }
        catch (Exception ex)
        {
            logger.LogException(ex, LogLevel.Warning);
            return null;
        }

        return null;
    }
}