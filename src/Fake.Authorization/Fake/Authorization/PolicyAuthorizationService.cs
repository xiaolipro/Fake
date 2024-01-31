using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Fake.Authorization.Localization;
using Fake.Identity.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Fake.Authorization;

/// <summary>
/// 基于Policy的授权服务
/// </summary>
/// <param name="currentPrincipalAccessor"></param>
/// <param name="authorizationService"></param>
/// <param name="authorizationPolicyProvider"></param>
public class PolicyAuthorizationService(
    ICurrentPrincipalAccessor currentPrincipalAccessor,
    IAuthorizationService authorizationService,
    IAuthorizationPolicyProvider authorizationPolicyProvider)
    : IMethodAuthorizationService
{
    public async Task CheckAsync(MethodAuthorizationContext context)
    {
        if (AllowAnonymous(context)) return;

        var authorizeData = GetAuthorizationDataAttributes(context.Method);
        var policy = await AuthorizationPolicy.CombineAsync(authorizationPolicyProvider, authorizeData);

        if (policy == null) return;

        if (currentPrincipalAccessor.Principal == null) return;

        var res = await authorizationService.AuthorizeAsync(
            currentPrincipalAccessor.Principal,
            null,
            policy
        );

        if (!res.Succeeded)
        {
            throw new FakeAuthorizationException(FakeAuthorizationResource.GivenPolicyHasNotGranted);
        }
    }

    private IEnumerable<IAuthorizeData> GetAuthorizationDataAttributes(MethodInfo invocationMethod)
    {
        var attributes = invocationMethod.GetCustomAttributes(true)
            .OfType<IAuthorizeData>();

        if (invocationMethod is { IsPublic: true, DeclaringType: not null })
        {
            attributes = attributes.Union(
                invocationMethod.DeclaringType.GetCustomAttributes(true).OfType<IAuthorizeData>()
            );
        }

        return attributes;
    }


    protected virtual bool AllowAnonymous(MethodAuthorizationContext context)
    {
        return context.Method.GetCustomAttributes(true).OfType<IAllowAnonymous>().Any();
    }
}