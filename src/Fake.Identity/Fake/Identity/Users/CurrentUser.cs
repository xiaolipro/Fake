using System;
using System.Linq;
using Fake.Identity.Security.Claims;
using JetBrains.Annotations;

namespace Fake.Identity.Users;

public class CurrentUser : ICurrentUser
{
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;
    public bool IsAuthenticated => UserId.IsNullOrWhiteSpace();
    public string UserId => FindClaimValue(FakeClaimTypes.UserId);
    public string UserName => FindClaimValue(FakeClaimTypes.UserName);

    public CurrentUser(ICurrentPrincipalAccessor currentPrincipalAccessor)
    {
        _currentPrincipalAccessor = currentPrincipalAccessor;
    }

    [CanBeNull]
    public virtual string FindClaimValue(string claimType)
    {
        return _currentPrincipalAccessor
            .Principal?
            .Claims
            .FirstOrDefault(c => c.Type == claimType)?
            .Value;
    }
}