namespace Fake.Authorization.Permissions.Contributors;

public abstract class PermissionCheckContributorBase(IPermissionStore permissionStore) : IPermissionCheckContributor
{
    protected readonly IPermissionStore PermissionStore = permissionStore;
    public virtual string PermissionOwner => "*";
    public abstract Task<bool> IsGrantedAsync(ClaimsPrincipal? user, PermissionDto permission);

    public virtual async Task<bool> IsGrantedAsync(ClaimsPrincipal? user, PermissionDto[] permissions)
    {
        foreach (var permission in permissions)
        {
            if (await IsGrantedAsync(user, permission)) return true;
        }

        return false;
    }
}