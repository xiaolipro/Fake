using System.Reflection;

namespace Bang.Modularity;

public abstract class BangModule:IBangModule
{
    public virtual bool IsBangFrameworkModule => false;
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


    public static bool IsBangModule(Type type)
    {
        var typeInfo = type.GetTypeInfo();

        return
            typeInfo.IsClass &&
            !typeInfo.IsAbstract &&
            !typeInfo.IsGenericType &&
            typeof(IBangModule).GetTypeInfo().IsAssignableFrom(type);
    }
}