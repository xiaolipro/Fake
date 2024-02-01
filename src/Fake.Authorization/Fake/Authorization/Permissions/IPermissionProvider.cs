namespace Fake.Authorization.Permissions;

public interface IPermissionProvider
{
    Task<List<Permission>> GetPermissionsAsync();
}