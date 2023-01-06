using System.Reflection;

namespace Bang.Modularity;

public abstract class BangModule:IBangModule
{
    public virtual bool IsBangFrameworkModule => false;
    public virtual bool SkipAutoServiceRegistration => false;
    
    public virtual void PreConfigServices(ServiceConfigurationContext context)
    {
    }

    public virtual void ConfigServices(ServiceConfigurationContext context)
    {
    }

    public virtual void PostConfigServices(ServiceConfigurationContext context)
    {
    }

    public virtual void PreConfigure(ApplicationInitializationContext context)
    {
    }

    public virtual void Configure(ApplicationInitializationContext context)
    {
    }

    public virtual void PostConfigure(ApplicationInitializationContext context)
    {
    }
    
    public virtual void PreShutDown(ApplicationShutdownContext context)
    {
    }

    public virtual void Shutdown(ApplicationShutdownContext context)
    {
    }

    public virtual void PostShutdown(ApplicationShutdownContext context)
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