using Fake.Modularity;

namespace Fake;

/// <summary>
/// FakeApp静态工厂
/// </summary>
public static class FakeApplicationFactory
{
    public static FakeApplication Create<TStartupModule>(
        Action<FakeApplicationCreationOptions>? optionsAction = null)
        where TStartupModule : IFakeModule
    {
        return Create(typeof(TStartupModule), optionsAction);
    }

    public static FakeApplication Create(
        Type startupModuleType,
        Action<FakeApplicationCreationOptions>? optionsAction = null)
    {
        return new FakeApplication(startupModuleType, optionsAction);
    }

    public static FakeApplication Create<TStartupModule>(
        IServiceCollection services,
        Action<FakeApplicationCreationOptions>? optionsAction = null)
        where TStartupModule : IFakeModule
    {
        return Create(typeof(TStartupModule), services, optionsAction);
    }

    public static FakeApplication Create(
        Type startupModuleType,
        IServiceCollection services,
        Action<FakeApplicationCreationOptions>? optionsAction = null)
    {
        return new FakeApplication(startupModuleType, services, optionsAction);
    }
}