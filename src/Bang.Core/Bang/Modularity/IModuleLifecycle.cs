namespace Bang.Modularity;

public interface IModuleLifecycle
{
    void PreConfigServices(ServiceConfigurationContext context);

    void ConfigServices(ServiceConfigurationContext context);

    void PostConfigServices(ServiceConfigurationContext context);
    
    void PreInitialization(ApplicationInitializationContext context);

    void Initialization(ApplicationInitializationContext context);

    void PostInitialization(ApplicationInitializationContext context);

    void PreShutDown(ApplicationInitializationContext context);

    void Shutdown(ApplicationShutdownContext context);
}