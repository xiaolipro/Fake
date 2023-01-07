using Bang.Modularity;

namespace Bang;

public class BangApplicationFactory
{
    public static BangApplication Create<TStartupModule>(
        [CanBeNull] Action<BangApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IBangModule
    {
        return Create(typeof(TStartupModule), optionsAction);
    }
    
    public static BangApplication Create(
        [NotNull] Type startupModuleType,
        [CanBeNull] Action<BangApplicationCreationOptions> optionsAction = null)
    {
        return new BangApplication(startupModuleType, optionsAction);
    }
    
    public static BangApplication Create<TStartupModule>(
        [NotNull] IServiceCollection services,
        [CanBeNull] Action<BangApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IBangModule
    {
        return Create(typeof(TStartupModule), services, optionsAction);
    }
    
    public static BangApplication Create(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        [CanBeNull] Action<BangApplicationCreationOptions> optionsAction = null)
    {
        return new BangApplication(startupModuleType, services, optionsAction);
    }
}