using Fake;
using Fake.Modularity;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeServiceCollectionApplicationExtensions
{
    public static FakeApplication AddFakeApplication<TStartupModule>(
        [NotNull] this IServiceCollection services,
        [CanBeNull] Action<FakeApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IFakeModule
    {
        return FakeApplicationFactory.Create<TStartupModule>(services, optionsAction);
    } 
    
    public static FakeApplication AddFakeApplication(
        [NotNull] this IServiceCollection services,
        [NotNull] Type startupModuleType,
        [CanBeNull] Action<FakeApplicationCreationOptions> optionsAction = null)
    {
        return FakeApplicationFactory.Create(startupModuleType, services, optionsAction);
    }
    
    [CanBeNull]
    public static string GetApplicationName(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IApplicationInfo>().ApplicationName;
    }
    
    [NotNull]
    public static string GetApplicationId(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IApplicationInfo>().ApplicationId;
    }
}