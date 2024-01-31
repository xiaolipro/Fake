using System.Security.Claims;
using System.Threading.Tasks;

namespace Fake.Authorization.Permissions;

/// <summary>
/// 免权限检测
/// </summary>
public class PassThroughPermissionChecker : IPermissionChecker
{
    public Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, PermissionRequirement requirement)
    {
        return Task.FromResult(true);
    }
}