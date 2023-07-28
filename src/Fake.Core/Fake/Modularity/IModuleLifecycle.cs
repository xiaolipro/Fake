namespace Fake.Modularity;

public interface IConfigureServicesLifecycle
{
    void PreConfigureServices(ServiceConfigurationContext context);

    void ConfigureServices(ServiceConfigurationContext context);

    void PostConfigureServices(ServiceConfigurationContext context);
}

public interface IConfigureApplicationLifecycle
{
    void PreConfigureApplication(ApplicationConfigureContext context);

    void ConfigureApplication(ApplicationConfigureContext context);

    void PostConfigureApplication(ApplicationConfigureContext context);
}

public interface IShutdownLifecycle
{
    void Shutdown(ApplicationShutdownContext context);
}