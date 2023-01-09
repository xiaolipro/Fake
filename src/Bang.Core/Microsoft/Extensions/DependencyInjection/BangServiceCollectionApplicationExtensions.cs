using Bang;
using Bang.Modularity;

namespace Microsoft.Extensions.DependencyInjection;

public static class BangServiceCollectionApplicationExtensions
{
    public static BangApplication AddStartupModule<TStartupModule>(
        [NotNull] this IServiceCollection services,
        [CanBeNull] Action<BangApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IBangModule
    {
        return BangApplicationFactory.Create<TStartupModule>(services, optionsAction);
    } 
    
    public static BangApplication AddApplication(
        [NotNull] this IServiceCollection services,
        [NotNull] Type startupModuleType,
        [CanBeNull] Action<BangApplicationCreationOptions> optionsAction = null)
    {
        return BangApplicationFactory.Create(startupModuleType, services, optionsAction);
    }
}