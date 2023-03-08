using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Fake.AspNetCore.Security;

public class FakeSecurityHeadersMiddleware : IMiddleware
{
    private readonly FakeSecurityHeadersOptions _options;

    public FakeSecurityHeadersMiddleware(IOptions<FakeSecurityHeadersOptions> options)
    {
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // XSS保护
        AddHeaderIfNotExists(context, "X-XSS-Protection", "1; mode=block");

        // 同源限制
        AddHeaderIfNotExists(context, "X-Frame-Options", "SAMEORIGIN");

        // 防止嗅探
        AddHeaderIfNotExists(context, "X-Content-Type-Options", "nosniff");

        if (_options.UseContentSecurityPolicyHeader)
        {
            AddHeaderIfNotExists(context, "Content-Security-Policy", _options.ContentSecurityPolicyValue);
        }
        
        await next.Invoke(context);
    }

    protected virtual void AddHeaderIfNotExists(HttpContext context, string key, string value)
    {
        context.Response.Headers.TryAdd(new KeyValuePair<string, StringValues>(key, value));
    }
}