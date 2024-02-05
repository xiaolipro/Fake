using System.Linq;
using System.Security.Claims;
using Fake.Identity.Security.Claims;

namespace Fake.Identity.Users;

public class CurrentUser : ICurrentUser
{
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

    // public virtual bool IsAuthenticated => !string.IsNullOrEmpty(this.m_authenticationType);
    public bool IsAuthenticated => _currentPrincipalAccessor.Principal?.Identity.IsAuthenticated ?? false;
    public string? UserId => FindClaimValueOrNull(ClaimTypes.NameIdentifier);
    public string? UserName => FindClaimValueOrNull(ClaimTypes.Name);

    public string? UserRole => FindClaimValueOrNull(ClaimTypes.Role);

    public CurrentUser(ICurrentPrincipalAccessor currentPrincipalAccessor)
    {
        _currentPrincipalAccessor = currentPrincipalAccessor;
    }


    public virtual string? FindClaimValueOrNull(string claimType)
    {
        return _currentPrincipalAccessor
            .Principal?
            .Claims
            .FirstOrDefault(c => c.Type == claimType)?.Value;
    }
}