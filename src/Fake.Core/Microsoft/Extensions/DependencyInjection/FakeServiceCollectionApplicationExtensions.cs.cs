using Fake;
using Fake.Modularity;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeServiceCollectionApplicationExtensions
{
    public static FakeApplication AddFakeApplication<TStartupModule>(
        this IServiceCollection services,
        [CanBeNull] Action<FakeApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IFakeModule
    {
        return FakeApplicationFactory.Create<TStartupModule>(services, optionsAction);
    }

    public static FakeApplication AddFakeApplication(
        this IServiceCollection services,
        Type startupModuleType,
        [CanBeNull] Action<FakeApplicationCreationOptions> optionsAction = null)
    {
        return FakeApplicationFactory.Create(startupModuleType, services, optionsAction);
    }

    [CanBeNull]
    public static string GetApplicationName(this IServiceCollection services)
    {
        return services.GetInstance<IApplicationInfo>().ApplicationName;
    }

    public static string GetApplicationId(this IServiceCollection services)
    {
        return services.GetInstance<IApplicationInfo>().ApplicationId;
    }
}