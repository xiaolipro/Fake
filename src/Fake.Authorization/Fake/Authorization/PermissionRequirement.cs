namespace Fake.Authorization;

/// <summary>
/// 权限Requirement
/// </summary>
public class PermissionRequirement(string permissionName) : IAuthorizationRequirement
{
    public string PermissionName { get; } = ThrowHelper.ThrowIfNull(permissionName, nameof(permissionName));
}