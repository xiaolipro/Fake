using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fake.Authorization.Permissions;

public class PermissionChecker : IPermissionChecker
{
    private readonly IPermissionRecordStore _permissionRecordStore;

    public async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, PermissionRequirement requirement)
    {
        ThrowHelper.ThrowIfNull(requirement.Permissions);

        foreach (var permission in requirement.Permissions)
        {
            var isGranted = await IsGrantedAsync(claimsPrincipal, permission);
            if (isGranted && !requirement.RequiresAll) return true;
            if (!isGranted) return false;
        }

        return true;
    }

    public async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string permission)
    {
        ThrowHelper.ThrowIfNull(permission);

        var permission = await PermissionDefinitionManager.GetOrNullAsync(name);
    }
}

public interface IPermissionRecordStore
{
    Task<PermissionRecord> GetOrNullAsync(string name);

    Task<IReadOnlyList<PermissionRecord>> GetPermissionsAsync();

    Task<IReadOnlyList<PermissionRecord>> GetGroupsAsync();
}

public record PermissionRecord
{
}