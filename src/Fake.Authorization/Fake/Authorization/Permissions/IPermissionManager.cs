namespace Fake.Authorization.Permissions;

public interface IPermissionManager
{
    Task<Permission?> GetOrNullAsync(string name);

    Task<IReadOnlyList<Permission>> GetPermissionsAsync();
}