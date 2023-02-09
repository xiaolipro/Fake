using System;
using System.Security.Claims;
using System.Threading;

namespace Fake.Identity.Security.Claims;

public abstract class AbstractCurrentPrincipalAccessor : ICurrentPrincipalAccessor
{
    private readonly AsyncLocal<ClaimsPrincipal> _currentPrincipal = new();
    public ClaimsPrincipal Principal => _currentPrincipal.Value ?? GetClaimsPrincipal();
    
    public virtual IDisposable Change(ClaimsPrincipal principal)
    {
        var parent = Principal;
        _currentPrincipal.Value = principal;
        return new DisposableWrapper(() =>
        {
            _currentPrincipal.Value = parent;
        });
    }
    
    /// <summary>
    /// 获取ClaimsPrincipal
    /// </summary>
    /// <returns></returns>
    protected abstract ClaimsPrincipal GetClaimsPrincipal();
}