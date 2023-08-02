using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Fake.AspNetCore.Http;

public class HttpClientInfoProvider:IHttpClientInfoProvider
{
    private readonly ILogger<HttpClientInfoProvider> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public string UserAgent => GetUserAgent();
    public string ClientIpAddress => GetClientIpAddress();

    public HttpClientInfoProvider( ILogger<HttpClientInfoProvider> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }
    
    protected virtual string GetUserAgent()
    {
        return _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"];
    }

    protected virtual string GetClientIpAddress()
    {
        try
        {
            if (_httpContextAccessor.HttpContext?.Connection != null)
                return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, LogLevel.Warning);
            return null;
        }

        return null;
    }
}