using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Fake.Identity.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Fake.Authorization;

public class MethodAuthorizationService : IMethodAuthorizationService
{
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;
    private readonly IAuthorizationService _authorizationService;
    private readonly IAuthorizationPolicyProvider _authorizationPolicyProvider;

    public MethodAuthorizationService(ICurrentPrincipalAccessor currentPrincipalAccessor,
        IAuthorizationService authorizationService, IAuthorizationPolicyProvider authorizationPolicyProvider)
    {
        _currentPrincipalAccessor = currentPrincipalAccessor;
        _authorizationService = authorizationService;
        _authorizationPolicyProvider = authorizationPolicyProvider;
    }

    public async Task<bool> IsGrantedAsync(MethodInfo invocationMethod)
    {
        if (AllowAnonymous(invocationMethod))
        {
            return true;
        }

        var authorizeData = GetAuthorizationDataAttributes(invocationMethod);
        var policy = await AuthorizationPolicy.CombineAsync(_authorizationPolicyProvider, authorizeData);
        if (policy == null)
        {
            return true;
        }

        var res = await _authorizationService.AuthorizeAsync(
            _currentPrincipalAccessor.Principal,
            null,
            policy
        );
        return res.Succeeded;
    }

    private IEnumerable<IAuthorizeData> GetAuthorizationDataAttributes(MethodInfo invocationMethod)
    {
        var attributes = invocationMethod.GetCustomAttributes(true)
            .OfType<IAuthorizeData>();

        if (invocationMethod.IsPublic && invocationMethod.DeclaringType != null)
        {
            attributes = attributes
                .Union(
                    invocationMethod.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<IAuthorizeData>()
                );
        }

        return attributes;
    }


    protected virtual bool AllowAnonymous(MethodInfo method)
    {
        return method.GetCustomAttributes(true).OfType<IAllowAnonymous>().Any();
    }
}