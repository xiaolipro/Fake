using System.Security.Claims;
using System.Threading.Tasks;

namespace Fake.Authorization.Permissions;

public class PermissionChecker(IPermissionRecordStore permissionRecordStore) : IPermissionChecker
{
    public async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, PermissionRequirement requirement)
    {
        ThrowHelper.ThrowIfNull(requirement.Permissions);

        foreach (var permission in requirement.Permissions)
        {
            var isGranted = await IsGrantedAsync(claimsPrincipal, permission);
            switch (isGranted)
            {
                case true when !requirement.RequiresAll:
                    return true;
                case false:
                    return false;
            }
        }

        return true;
    }

    public async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string permission)
    {
        ThrowHelper.ThrowIfNull(permission);

        var record = await permissionRecordStore.GetOrNullAsync(permission);

        if (record == null) return false;

        return true;
    }
}