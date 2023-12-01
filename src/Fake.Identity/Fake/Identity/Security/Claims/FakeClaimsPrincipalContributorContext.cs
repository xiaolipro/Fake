using System;
using System.Security.Claims;

namespace Fake.Identity.Security.Claims;

public class FakeClaimsPrincipalContributorContext
{
    public ClaimsPrincipal ClaimsPrincipal { get; }

    public IServiceProvider ServiceProvider { get; }

    public FakeClaimsPrincipalContributorContext(ClaimsPrincipal claimsPrincipal, IServiceProvider serviceProvider)
    {
        ClaimsPrincipal = claimsPrincipal;
        ServiceProvider = serviceProvider;
    }
}