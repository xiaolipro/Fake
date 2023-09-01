using Microsoft.AspNetCore.Authorization;

namespace Fake.Authorization.Permissions;

/// <summary>
///     权限
/// </summary>
public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(params string[] permissions)
    {
        Permissions = permissions;
    }

    public PermissionRequirement(bool requiresAll, params string[] permissions)
    {
        RequiresAll = requiresAll;
        Permissions = permissions;
    }

    public bool RequiresAll { get; }
    public string[] Permissions { get; }
}