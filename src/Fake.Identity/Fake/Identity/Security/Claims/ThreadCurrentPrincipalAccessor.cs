using System.Security.Claims;
using System.Threading;
using Fake.DependencyInjection;

namespace Fake.Identity.Security.Claims;

/// <summary>
/// 基于Thread访问ClaimsPrincipal
/// </summary>
public class ThreadCurrentPrincipalAccessor : AbstractCurrentPrincipalAccessor, ISingletonDependency
{
    protected override ClaimsPrincipal GetClaimsPrincipal()
    {
        return Thread.CurrentPrincipal as ClaimsPrincipal;
    }
}