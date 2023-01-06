namespace Bang.Modularity;

public interface IModuleLifecycle : IConfigureServicesLifecycle, IInitializationLifecycle, IShutdownLifecycle
{
}

public interface IConfigureServicesLifecycle
{
    void PreConfigServices(ServiceConfigurationContext context);

    void ConfigServices(ServiceConfigurationContext context);

    void PostConfigServices(ServiceConfigurationContext context);
}

public interface IInitializationLifecycle
{
    void PreConfigure(ApplicationInitializationContext context);

    void Configure(ApplicationInitializationContext context);

    void PostConfigure(ApplicationInitializationContext context);
}

public interface IShutdownLifecycle
{
    void Shutdown(ApplicationShutdownContext context);
}