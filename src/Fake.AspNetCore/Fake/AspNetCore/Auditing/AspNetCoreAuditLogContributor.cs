using System;
using Fake.AspNetCore.Http;
using Fake.Auditing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.AspNetCore.Auditing;

public class AspNetCoreAuditLogContributor:AuditLogContributor
{
    public override void PreContribute(AuditLogContributionContext context)
    {
        var httpContext = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        
        if (httpContext == null) return;
        
        if (httpContext.WebSockets.IsWebSocketRequest) return;

        context.AuditInfo.HttpMethod ??= httpContext.Request.Method;
        context.AuditInfo.Url = BuildUrl(httpContext);

        var httpClientInfoProvider = context.ServiceProvider.GetRequiredService<IHttpClientInfoProvider>();
        context.AuditInfo.ClientIpAddress = httpClientInfoProvider.ClientIpAddress;
        context.AuditInfo.UserAgent = httpClientInfoProvider.UserAgent;
    }

    public override void PostContribute(AuditLogContributionContext context)
    {
        if (context.AuditInfo.HttpStatusCode != null) return;
        
        var httpContext = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        if (httpContext == null) return;
        
        context.AuditInfo.HttpStatusCode = httpContext.Response.StatusCode;
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