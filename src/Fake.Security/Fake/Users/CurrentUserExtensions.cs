using System.Linq;

namespace Fake.Users;

public static class CurrentUserExtensions
{
    public static string? FindClaimValueOrNull(this ICurrentUser currentUser, string claimType)
    {
        return currentUser.FindClaimOrNull(claimType)?.Value;
    }

    public static string[] FindClaimValues(this ICurrentUser currentUser, string claimType)
    {
        return currentUser.FindClaims(claimType).Select(c => c.Value).ToArray();
    }
}