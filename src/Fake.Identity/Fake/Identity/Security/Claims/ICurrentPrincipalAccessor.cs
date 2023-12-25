using System;
using System.Security.Claims;

namespace Fake.Identity.Security.Claims;

public interface ICurrentPrincipalAccessor
{
    /// <summary>
    /// 获取当前ClaimsPrincipal
    /// </summary>
    ClaimsPrincipal? Principal { get; }

    /// <summary>
    /// 切换当前ClaimsPrincipal，dispose时还原到之前
    /// </summary>
    /// <param name="principal"></param>
    /// <returns></returns>
    IDisposable Change(ClaimsPrincipal principal);
}