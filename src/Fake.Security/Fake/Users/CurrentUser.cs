using System.Linq;
using System.Security.Claims;
using Fake.Security.Claims;

namespace Fake.Users;

public class CurrentUser(ICurrentPrincipalAccessor currentPrincipalAccessor) : ICurrentUser
{
    /*
     * see: public virtual bool IsAuthenticated => !string.IsNullOrEmpty(this.m_authenticationType);
     */
    public bool IsAuthenticated => currentPrincipalAccessor.Principal?.Identity.IsAuthenticated ?? false;
    public string? UserId => this.FindClaimValueOrNull(ClaimTypes.NameIdentifier);
    public string? UserName => this.FindClaimValueOrNull(ClaimTypes.Name);
    public string? Roles => this.FindClaimValueOrNull(ClaimTypes.Role);

    public virtual Claim? FindClaimOrNull(string claimType)
    {
        return currentPrincipalAccessor
            .Principal?
            .Claims
            .FirstOrDefault(c => c.Type == claimType);
    }
}