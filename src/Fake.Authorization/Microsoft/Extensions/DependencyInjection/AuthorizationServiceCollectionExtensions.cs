using Fake.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeAuthorizationServiceCollectionExtensions
{
    public static IServiceCollection AddPassThroughAuthorization(this IServiceCollection services)
    {
        return services
            .Replace(ServiceDescriptor.Singleton<IAuthorizationService, PassThroughAuthorizationService>());
    }
}