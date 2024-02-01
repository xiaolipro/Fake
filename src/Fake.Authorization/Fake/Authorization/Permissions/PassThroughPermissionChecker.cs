namespace Fake.Authorization.Permissions;

/// <summary>
/// 免权限检测
/// </summary>
public class PassThroughPermissionChecker : IPermissionChecker
{
    public Task<bool> IsGrantedAsync(params string[] permissions)
    {
        return Task.FromResult(true);
    }

    public Task<bool> IsGrantedAsync(PermissionRequirement requirement)
    {
        return Task.FromResult(true);
    }
}