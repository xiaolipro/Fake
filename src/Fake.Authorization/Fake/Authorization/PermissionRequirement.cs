using Microsoft.AspNetCore.Authorization;

namespace Fake.Authorization;

/// <summary>
/// 权限Requirement
/// </summary>
public class PermissionRequirement(bool requiresAll = true, params string[] permissions) : IAuthorizationRequirement
{
    public bool RequiresAll { get; } = requiresAll;
    public string[] Permissions { get; } = permissions;

    public PermissionRequirement(params string[] permissions) : this(false, permissions)
    {
    }
}