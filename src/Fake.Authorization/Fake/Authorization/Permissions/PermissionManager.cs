using System.Collections.Immutable;
using System.Threading;

namespace Fake.Authorization.Permissions;

public class PermissionManager(IEnumerable<IPermissionDefiner> permissionProviders) : IPermissionManager
{
    private Dictionary<string, PermissionDto>? _permissions;
    private static readonly SemaphoreSlim SemaphoreSlim = new(1, 1);

    public async Task<PermissionDto?> GetOrNullAsync(string permissionName)
    {
        await EnsureInitialized();
        return _permissions!.GetOrDefault(permissionName);
    }

    public async Task<IReadOnlyList<PermissionDto>> GetPermissionsAsync()
    {
        await EnsureInitialized();
        return _permissions!.Values.ToImmutableList();
    }

    protected virtual async Task EnsureInitialized()
    {
        using (SemaphoreSlim.Lock())
        {
            if (_permissions != null) return;

            _permissions = new Dictionary<string, PermissionDto>();

            foreach (var permissionProvider in permissionProviders)
            {
                var permissions = await permissionProvider.GetPermissionsAsync();

                foreach (var permission in permissions)
                {
                    AddPermissionRecursively(_permissions, permission);
                }
            }
        }
    }

    protected virtual void AddPermissionRecursively(
        Dictionary<string, PermissionDto> permissions,
        PermissionDto permissionDto)
    {
        if (permissions.ContainsKey(permissionDto.Name))
        {
            throw new FakeException("Duplicate permissionDto name: " + permissionDto.Name);
        }

        permissions[permissionDto.Name] = permissionDto;

        foreach (var child in permissionDto.Children)
        {
            AddPermissionRecursively(permissions, child);
        }
    }
}