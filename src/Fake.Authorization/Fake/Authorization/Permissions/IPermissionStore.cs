namespace Fake.Authorization.Permissions;

public interface IPermissionStore
{
    public Task<bool> IsGrantedAsync(
        string permission,
        string ownerName,
        string ownerKey
    );
}