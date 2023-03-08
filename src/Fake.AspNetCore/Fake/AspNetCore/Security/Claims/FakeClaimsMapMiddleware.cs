using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fake.Identity.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fake.AspNetCore.Security.Claims;

/// <summary>
/// <see cref="Claim"/>映射中间件
/// </summary>
public class FakeClaimsMapMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        var serviceProvider = httpContext.RequestServices;

        var currentPrincipalAccessor = serviceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
        var maps = serviceProvider.GetRequiredService<IOptions<FakeClaimsMapOptions>>().Value.Maps;

        var mapClaims = currentPrincipalAccessor
            .Principal
            .Claims
            .Where(c => c.Type.In(maps.Keys));

        currentPrincipalAccessor.Principal.AddIdentity(
            new ClaimsIdentity(
                mapClaims.Select(c => new Claim(
                    maps[c.Type], c.Value, c.ValueType, c.Issuer
                    )
                )
            )
        );

        await next(httpContext);
    }
}