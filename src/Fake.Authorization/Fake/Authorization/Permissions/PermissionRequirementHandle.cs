using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Fake.Authorization.Permissions;

public class PermissionRequirementHandle : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        throw new NotImplementedException();
    }
}

public interface IPermissionValidator
{
    Task<PermissionGrantResult>
}

public class PermissionGrantResult
{
}