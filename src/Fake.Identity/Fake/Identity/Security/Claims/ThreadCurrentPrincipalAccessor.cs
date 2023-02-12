using System.Security.Claims;
using System.Threading;
using Fake.DependencyInjection;

namespace Fake.Identity.Security.Claims;

public class ThreadCurrentPrincipalAccessor : AbstractCurrentPrincipalAccessor, ISingletonDependency
{
    protected override ClaimsPrincipal GetClaimsPrincipal()
    {
        return Thread.CurrentPrincipal as ClaimsPrincipal;
    }
}