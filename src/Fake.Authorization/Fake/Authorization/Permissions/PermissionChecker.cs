using Fake.Authorization.Permissions.Contributors;
using Fake.Security.Claims;

namespace Fake.Authorization.Permissions;

public class PermissionChecker(
    ICurrentPrincipalAccessor principalAccessor,
    IPermissionManager permissionManager,
    IEnumerable<IPermissionCheckContributor> contributors)
    : IPermissionChecker
{
    public async Task<bool> IsGrantedAsync(params string[] permissionNames)
    {
        var permissions = new List<PermissionDto>();
        foreach (var permissionName in permissionNames)
        {
            var permission = await permissionManager.GetOrNullAsync(permissionName);
            if (permission == null)
            {
                return false;
            }

            permissions.Add(permission);
        }

        foreach (var contributor in contributors)
        {
            if (await contributor.IsGrantedAsync(principalAccessor.Principal, permissions.ToArray()))
            {
                return true;
            }
        }

        return false;
    }

    public async Task<bool> IsGrantedAsync(ClaimsPrincipal? user, PermissionDto permission)
    {
        foreach (var contributor in contributors)
        {
            if (await contributor.IsGrantedAsync(user, permission))
            {
                return true;
            }
        }

        return false;
    }
}