using System.Security.Claims;
using Fake.Identity.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Fake.AspNetCore.Security.Claims;

/// <summary>
/// 基于HttpContext访问ClaimsPrincipal
/// </summary>
public class HttpContextCurrentPrincipalAccessor(IHttpContextAccessor httpContextAccessor)
    : ThreadCurrentPrincipalAccessor
{
    protected override ClaimsPrincipal? GetClaimsPrincipal()
    {
        // 如果无法从HttpContext获取，则降级去Thread获取
        return httpContextAccessor.HttpContext?.User ?? base.GetClaimsPrincipal();
    }
}