using System.Reflection;

namespace Fake.Modularity;

public abstract class FakeModule:IFakeModule
{
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