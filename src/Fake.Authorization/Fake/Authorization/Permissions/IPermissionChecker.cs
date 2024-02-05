namespace Fake.Authorization.Permissions;

public interface IPermissionChecker
{
    Task<bool> IsGrantedAsync(params string[] permissionNames);
    Task<bool> IsGrantedAsync(ClaimsPrincipal? user, PermissionDto permission);
}