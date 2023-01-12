using System.Reflection;

namespace Fake.Modularity;

public abstract class FakeModule:IFakeModule
{
    public virtual bool IsFakeFrameworkModule => false;
    public virtual bool SkipAutoServiceRegistration => false;
    
    public virtual void PreConfigureServices(ServiceConfigurationContext context)
    {
    }

    public virtual void ConfigureServices(ServiceConfigurationContext context)
    {
    }

    public virtual void PostConfigureServices(ServiceConfigurationContext context)
    {
    }

    public virtual void PreConfigure(ApplicationConfigureContext context)
    {
    }

    public virtual void Configure(ApplicationConfigureContext context)
    {
    }

    public virtual void PostConfigure(ApplicationConfigureContext context)
    {
    }

    public virtual void Shutdown(ApplicationShutdownContext context)
    {
    }


    public static bool IsFakeModule(Type type)
    {
        var typeInfo = type.GetTypeInfo();

        return
            typeInfo.IsClass &&
            !typeInfo.IsAbstract &&
            !typeInfo.IsGenericType &&
            typeof(IFakeModule).GetTypeInfo().IsAssignableFrom(type);
    }
}