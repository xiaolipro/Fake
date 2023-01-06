namespace Bang.Modularity;

public interface IModuleLifecycle : IConfigureServicesLifecycle, IConfigureLifecycle, IShutdownLifecycle
{
}

public interface IConfigureServicesLifecycle
{
    void PreConfigureServices(ServiceConfigurationContext context);

    void ConfigureServices(ServiceConfigurationContext context);

    void PostConfigureServices(ServiceConfigurationContext context);
}

public interface IConfigureLifecycle
{
    void PreConfigure(ApplicationConfigureContext context);

    void Configure(ApplicationConfigureContext context);

    void PostConfigure(ApplicationConfigureContext context);
}

public interface IShutdownLifecycle
{
    void Shutdown(ApplicationShutdownContext context);
}