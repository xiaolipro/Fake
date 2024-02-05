using System.Linq;

namespace Fake.Authorization.Permissions.Contributors;

public class RolePermissionCheckContributor(IPermissionStore permissionStore)
    : PermissionCheckContributorBase(permissionStore)
{
    public override string PermissionOwner => "Role";

    public override async Task<bool> IsGrantedAsync(ClaimsPrincipal? user, PermissionDto permission)
    {
        ThrowHelper.ThrowIfNull(permission, nameof(permission));

        var roles = user?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();
        if (roles == null || !roles.Any()) return false;

        foreach (var role in roles.Distinct())
        {
            if (await PermissionStore.IsGrantedAsync(permission.Name, PermissionOwner, role))
            {
                return true;
            }
        }

        return false;
    }
}