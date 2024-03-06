using System.Linq;
using Fake;

namespace System.Security.Claims;

public static class FakeClaimsPrincipalExtensions
{
    public static Guid? FindUserId(this ClaimsPrincipal principal)
    {
        ThrowHelper.ThrowIfNull(principal, nameof(principal));

        var userId = principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userId == null || userId.Value.IsNullOrWhiteSpace())
        {
            return null;
        }

        if (Guid.TryParse(userId.Value, out var guid))
        {
            return guid;
        }

        return null;
    }
}