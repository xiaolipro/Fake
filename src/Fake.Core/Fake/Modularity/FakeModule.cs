namespace Fake.Modularity;

public abstract class FakeModule : IFakeModule
{
    /// <summary>
    ///     当赋值为true时 跳过Fake提供的自动服务注册
    /// </summary>
    public virtual bool SkipServiceRegistration => false;

    public virtual void PreConfigureServices(ServiceConfigurationContext context)
    {
    }

    public virtual void ConfigureServices(ServiceConfigurationContext context)
    {
    }

    public virtual void PostConfigureServices(ServiceConfigurationContext context)
    {
    }

    public virtual void PreConfigureApplication(ApplicationConfigureContext context)
    {
    }

    public virtual void ConfigureApplication(ApplicationConfigureContext context)
    {
    }

    public virtual void PostConfigureApplication(ApplicationConfigureContext context)
    {
    }

    public virtual void Shutdown(ApplicationShutdownContext context)
    {
    }
}