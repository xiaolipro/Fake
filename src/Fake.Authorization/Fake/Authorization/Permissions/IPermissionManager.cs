namespace Fake.Authorization.Permissions;

public interface IPermissionManager
{
    Task<PermissionDto?> GetOrNullAsync(string permissionName);

    Task<IReadOnlyList<PermissionDto>> GetPermissionsAsync();
}