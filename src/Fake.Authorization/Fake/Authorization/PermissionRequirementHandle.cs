using Fake.Authorization.Permissions;

namespace Fake.Authorization;

public class PermissionRequirementHandle(
    IPermissionManager permissionManager,
    IPermissionChecker permissionChecker)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        ThrowHelper.ThrowIfNull(requirement.PermissionName);

        var isGranted = await IsGrantedAsync(context.User, requirement.PermissionName);
        if (isGranted) context.Succeed(requirement);
    }

    private async Task<bool> IsGrantedAsync(ClaimsPrincipal? claimsPrincipal, string permission)
    {
        var permissionDto = await permissionManager.GetOrNullAsync(permission);

        if (permissionDto == null) return false;
        if (permissionDto.IsEnabled == false) return false;

        return await permissionChecker.IsGrantedAsync(claimsPrincipal, permissionDto);
    }
}