using System.Security.Claims;
using System.Threading.Tasks;

namespace Fake.Authorization.Permissions;

public interface IPermissionChecker
{
    Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, PermissionRequirement requirement);
}