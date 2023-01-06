namespace Bang.Modularity;

public interface IModuleManager
{
    Task ConfigureServicesAsync([NotNull] ServiceConfigurationContext context);
    Task InitializeModulesAsync([NotNull] ApplicationConfigureContext context);
    Task ShutdownModulesAsync([NotNull] ApplicationShutdownContext context);
}