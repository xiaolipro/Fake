using System.Security.Claims;
using System.Threading.Tasks;
using Fake.Identity.Security.Claims;

namespace Fake.Authorization.Permissions;

public class PermissionChecker(
    IPermissionRecordStore permissionRecordStore,
    ICurrentPrincipalAccessor currentPrincipalAccessor) : IPermissionChecker
{
    public async Task<bool> IsGrantedAsync(PermissionRequirement requirement)
    {
        ThrowHelper.ThrowIfNull(requirement.Permissions);

        foreach (var permission in requirement.Permissions)
        {
            var isGranted = await IsGrantedAsync(currentPrincipalAccessor.Principal, permission);
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

    public async Task<bool> IsGrantedAsync(params string[] permissions)
    {
        return await IsGrantedAsync(currentPrincipalAccessor.Principal, permissions);
    }

    public async Task<bool> IsGrantedAsync(ClaimsPrincipal? claimsPrincipal, string[] permissions)
    {
        ThrowHelper.ThrowIfNull(permissions);

        foreach (var permission in permissions)
        {
            var isGranted = await IsGrantedAsync(claimsPrincipal, permission);
            if (isGranted == false) return false;
        }

        return true;
    }

    public async Task<bool> IsGrantedAsync(ClaimsPrincipal? claimsPrincipal, string permission)
    {
        ThrowHelper.ThrowIfNull(permission);

        var record = await permissionRecordStore.GetOrNullAsync(permission);

        return record != null;
    }
}