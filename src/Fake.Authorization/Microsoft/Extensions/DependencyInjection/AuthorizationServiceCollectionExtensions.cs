using Fake.Authorization;
using Fake.Authorization.Permissions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeAuthorizationServiceCollectionExtensions
{
    public static IServiceCollection AddPassThroughAuthorization(this IServiceCollection services)
    {
        return services
            .Replace(ServiceDescriptor.Singleton<IAuthorizationService, PassThroughAuthorizationService>())
            .Replace(ServiceDescriptor.Singleton<IPermissionChecker, PassThroughPermissionChecker>());
    }
}