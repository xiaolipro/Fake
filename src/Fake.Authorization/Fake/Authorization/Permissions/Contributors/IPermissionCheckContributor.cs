namespace Fake.Authorization.Permissions.Contributors;

public interface IPermissionCheckContributor
{
    string PermissionOwner { get; }
    Task<bool> IsGrantedAsync(ClaimsPrincipal? user, PermissionDto permission);
    Task<bool> IsGrantedAsync(ClaimsPrincipal? user, PermissionDto[] permissions);
}