using Fake;
using Fake.Modularity;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeApplicationServiceCollectionExtensions
{
    public static FakeApplication AddApplication<TStartupModule>(
        this IServiceCollection services,
        Action<FakeApplicationCreationOptions>? optionsAction = null)
        where TStartupModule : IFakeModule
    {
        return FakeApplicationFactory.Create<TStartupModule>(services, optionsAction);
    }

    public static FakeApplication AddApplication(
        this IServiceCollection services,
        Type startupModuleType,
        Action<FakeApplicationCreationOptions>? optionsAction = null)
    {
        return FakeApplicationFactory.Create(startupModuleType, services, optionsAction);
    }


    public static string GetApplicationName(this IServiceCollection services)
    {
        return services.GetInstance<IApplicationInfo>().ApplicationName;
    }

    public static string GetApplicationId(this IServiceCollection services)
    {
        return services.GetInstance<IApplicationInfo>().ApplicationId;
    }
}