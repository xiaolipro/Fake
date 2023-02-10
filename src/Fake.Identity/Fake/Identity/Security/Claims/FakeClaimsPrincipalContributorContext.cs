using System;
using System.Security.Claims;
using JetBrains.Annotations;

namespace Fake.Identity.Security.Claims;

public class FakeClaimsPrincipalContributorContext
{
    [NotNull]
    public ClaimsPrincipal ClaimsPrincipal { get; }
    [NotNull]
    public IServiceProvider ServiceProvider { get; }

    public FakeClaimsPrincipalContributorContext([NotNull] ClaimsPrincipal claimsPrincipal, [NotNull] IServiceProvider serviceProvider)
    {
        ClaimsPrincipal = claimsPrincipal;
        ServiceProvider = serviceProvider;
    }
}