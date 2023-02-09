using System.Security.Claims;
using System.Threading;

namespace Fake.Identity.Security.Claims;

public class ThreadCurrentPrincipalAccessor:AbstractCurrentPrincipalAccessor
{
    protected override ClaimsPrincipal GetClaimsPrincipal()
    {
        return Thread.CurrentPrincipal as ClaimsPrincipal;
    }
}