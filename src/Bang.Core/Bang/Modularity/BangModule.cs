using System.Reflection;

namespace Bang.Modularity;

public abstract class BangModule:IBangModule
{
    public virtual bool IsBangFrameworkModule => false;
    public virtual bool SkipAutoRegistrationService => false;
    
    public virtual void PreConfigServices(ServiceConfigurationContext context)
    {
    }

    public virtual void ConfigServices(ServiceConfigurationContext context)
    {
    }

    public virtual void PostConfigServices(ServiceConfigurationContext context)
    {
    }

    public virtual void PreInitialization(ApplicationInitializationContext context)
    {
    }

    public virtual void Initialization(ApplicationInitializationContext context)
    {
    }

    public virtual void PostInitialization(ApplicationInitializationContext context)
    {
    }

    public virtual void PreShutDown(ApplicationInitializationContext context)
    {
    }

    public virtual void Shutdown(ApplicationShutdownContext context)
    {
    }
    
    
    public static bool IsAbpModule(Type type)
    {
        var typeInfo = type.GetTypeInfo();

        return
            typeInfo.IsClass &&
            !typeInfo.IsAbstract &&
            !typeInfo.IsGenericType &&
            typeof(IBangModule).GetTypeInfo().IsAssignableFrom(type);
    }
}