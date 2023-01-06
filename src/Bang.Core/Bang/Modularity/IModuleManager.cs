namespace Bang.Modularity;

public interface IModuleManager
{
    Task ConfigureServicesAsync([NotNull] ServiceConfigurationContext context);
    Task InitializeModulesAsync([NotNull] ApplicationInitializationContext context);
    Task ShutdownModulesAsync([NotNull] ApplicationShutdownContext context);
}