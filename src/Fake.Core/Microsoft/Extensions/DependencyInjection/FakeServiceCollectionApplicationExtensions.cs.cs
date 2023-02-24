using Fake;
using Fake.Modularity;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeServiceCollectionApplicationExtensions
{
    public static FakeApplication AddStartupModule<TStartupModule>(
        [NotNull] this IServiceCollection services,
        [CanBeNull] Action<FakeApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IFakeModuleApplication
    {
        return FakeApplicationFactory.Create<TStartupModule>(services, optionsAction);
    } 
    
    public static FakeApplication AddStartupModule(
        [NotNull] this IServiceCollection services,
        [NotNull] Type startupModuleType,
        [CanBeNull] Action<FakeApplicationCreationOptions> optionsAction = null)
    {
        return FakeApplicationFactory.Create(startupModuleType, services, optionsAction);
    }
}