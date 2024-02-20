namespace Fake.Users;

public static class CurrentUserExtensions
{
    public static string? FindClaimValueOrNull(this ICurrentUser currentUser, string claimType)
    {
        return currentUser.FindClaimOrNull(claimType)?.Value;
    }
}