﻿using System.Threading.Tasks;
using Fake.Authorization.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace Fake.Authorization;

public class PermissionRequirementHandle : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionChecker _permissionChecker;

    public PermissionRequirementHandle(IPermissionChecker permissionChecker)
    {
        _permissionChecker = permissionChecker;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var claimsPrincipal = context.User;
        var isGranted = await _permissionChecker.IsGrantedAsync(claimsPrincipal, requirement);
        if (isGranted) context.Succeed(requirement);
    }
}