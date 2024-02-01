using System.Collections.Immutable;
using System.Threading;

namespace Fake.Authorization.Permissions;

public class PermissionManager(IEnumerable<IPermissionProvider> permissionProviders) : IPermissionManager
{
    private Dictionary<string, Permission>? _permissions;
    private static readonly SemaphoreSlim SemaphoreSlim = new(1, 1);

    public async Task<Permission?> GetOrNullAsync(string name)
    {
        await EnsureInitialized();
        return _permissions!.GetOrDefault(name);
    }

    public async Task<IReadOnlyList<Permission>> GetPermissionsAsync()
    {
        await EnsureInitialized();
        return _permissions!.Values.ToImmutableList();
    }

    protected virtual async Task EnsureInitialized()
    {
        using (SemaphoreSlim.Lock())
        {
            if (_permissions != null) return;

            _permissions = new Dictionary<string, Permission>();

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
        Dictionary<string, Permission> permissions,
        Permission permission)
    {
        if (permissions.ContainsKey(permission.Name))
        {
            throw new FakeException("Duplicate permission name: " + permission.Name);
        }

        permissions[permission.Name] = permission;

        foreach (var child in permission.Children)
        {
            AddPermissionRecursively(permissions, child);
        }
    }
}