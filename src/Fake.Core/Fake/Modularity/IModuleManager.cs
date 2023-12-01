namespace Fake.Modularity;

public interface IModuleManager
{
    Task ConfigureServicesAsync(ServiceConfigurationContext context);
    Task InitializeModulesAsync(ApplicationConfigureContext context);
    Task ShutdownModulesAsync(ApplicationShutdownContext context);
}