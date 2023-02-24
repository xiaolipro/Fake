using Fake.Modularity;

namespace Fake;

/// <summary>
/// FakeApp工厂
/// </summary>
public static class FakeApplicationFactory
{
    public static FakeApplication Create<TStartupModule>(
        [CanBeNull] Action<FakeApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IFakeModuleApplication
    {
        return Create(typeof(TStartupModule), optionsAction);
    }
    
    public static FakeApplication Create(
        [NotNull] Type startupModuleType,
        [CanBeNull] Action<FakeApplicationCreationOptions> optionsAction = null)
    {
        return new FakeApplication(startupModuleType, optionsAction);
    }
    
    public static FakeApplication Create<TStartupModule>(
        [NotNull] IServiceCollection services,
        [CanBeNull] Action<FakeApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IFakeModuleApplication
    {
        return Create(typeof(TStartupModule), services, optionsAction);
    }
    
    public static FakeApplication Create(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        [CanBeNull] Action<FakeApplicationCreationOptions> optionsAction = null)
    {
        return new FakeApplication(startupModuleType, services, optionsAction);
    }
}