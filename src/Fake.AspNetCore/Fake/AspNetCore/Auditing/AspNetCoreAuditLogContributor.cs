using Fake.AspNetCore.Http;
using Fake.Auditing;
using Fake.Data;
using Microsoft.AspNetCore.Http;

namespace Fake.AspNetCore.Auditing;

public class AspNetCoreAuditLogContributor : AuditLogContributor
{
    public const string HttpSimple = nameof(HttpSimple);
    public const string UserAgent = nameof(UserAgent);
    public const string ClientIpAddress = nameof(ClientIpAddress);
    public const string TraceIdentifier = nameof(TraceIdentifier);

    public override void PreContribute(AuditLogContributionContext context)
    {
        var httpContext = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;

        if (httpContext == null) return;

        if (httpContext.WebSockets.IsWebSocketRequest) return;

        context.AuditInfo.AddExtraProperties(TraceIdentifier, httpContext.TraceIdentifier);
        context.AuditInfo.AddExtraProperties(HttpSimple, httpContext.Request.ToString());

        var httpClientInfoProvider = context.ServiceProvider.GetRequiredService<IHttpClientInfoProvider>();
        context.AuditInfo.AddExtraProperties(ClientIpAddress, httpClientInfoProvider.ClientIpAddress);
        context.AuditInfo.AddExtraProperties(UserAgent, httpClientInfoProvider.UserAgent);
    }

    public override void PostContribute(AuditLogContributionContext context)
    {
        var httpContext = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        if (httpContext == null) return;

        if (context.AuditInfo.HasExtraProperty(HttpStatusCode))
        {
            context.AuditInfo.ExtraProperties[HttpStatusCode] ??= httpContext.Response.StatusCode;
        }

        if (context.AuditInfo.HasExtraProperty(TraceIdentifier))
        {
            context.AuditInfo.ExtraProperties[TraceIdentifier] ??= httpContext.TraceIdentifier;
        }
    }

    protected virtual string BuildUrl(HttpContext httpContext)
    {
        var uriBuilder = new UriBuilder
        {
            Scheme = httpContext.Request.Scheme,
            Host = httpContext.Request.Host.Host,
            Path = httpContext.Request.Path.ToString(),
            Query = httpContext.Request.QueryString.ToString()
        };

        return uriBuilder.Uri.AbsolutePath;
    }
}