﻿using Microsoft.AspNetCore.Authorization;

namespace Fake.Authorization;

/// <summary>
/// 权限Requirement
/// </summary>
public class PermissionRequirement : IAuthorizationRequirement
{
    public bool RequiresAll { get; }
    public string[] Permissions { get; }

    public PermissionRequirement(params string[] permissions)
    {
        Permissions = permissions;
    }

    public PermissionRequirement(bool requiresAll = true, params string[] permissions)
    {
        RequiresAll = requiresAll;
        Permissions = permissions;
    }
}