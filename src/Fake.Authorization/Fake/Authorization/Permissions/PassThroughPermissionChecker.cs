using System.Security.Claims;
using System.Threading.Tasks;

namespace Fake.Authorization.Permissions;

public class PassThroughPermissionChecker : IPermissionChecker
{
    public Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, PermissionRequirement requirement)
    {
        return Task.FromResult(true);
    }
}