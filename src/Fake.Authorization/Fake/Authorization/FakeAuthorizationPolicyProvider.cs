using Fake.Authorization.Permissions;
using Microsoft.Extensions.Options;

namespace Fake.Authorization;

public class FakeAuthorizationPolicyProvider(
    IOptions<AuthorizationOptions> options,
    IPermissionManager permissionManager)
    : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        // global defined policy
        var policy = await base.GetPolicyAsync(policyName);
        if (policy != null) return policy;

        // map form permission: policyName is permission name
        var permission = await permissionManager.GetOrNullAsync(policyName);

        if (permission == null) return null;

        var policyBuilder = new AuthorizationPolicyBuilder();
        policyBuilder.Requirements.Add(new PermissionRequirement(policyName));
        return policyBuilder.Build();
    }
}