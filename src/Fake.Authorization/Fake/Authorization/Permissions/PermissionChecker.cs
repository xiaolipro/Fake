using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fake.Authorization.Permissions;

public class PermissionChecker : IPermissionChecker
{
    public Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, PermissionRequirement requirement)
    {
        throw new NotImplementedException();
    }
}