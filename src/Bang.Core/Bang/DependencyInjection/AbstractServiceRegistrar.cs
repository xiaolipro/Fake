using System.Reflection;
using Bang.Helpers;

namespace Bang.DependencyInjection;

public abstract class AbstractServiceRegistrar : IServiceRegistrar
{
    public virtual void AddAssembly(IServiceCollection services, Assembly assembly)
    {
        var types = AssemblyHelper
            .GetAllTypes(assembly)
            .Where(
                type => type is { IsClass: true, IsAbstract: false, IsGenericType: false }
            ).ToArray();

        AddTypes(services, types);
    }

    public virtual void AddTypes(IServiceCollection services, params Type[] types)
    {
        foreach (var type in types)
        {
            AddType(services, type);
        }
    }

    public abstract void AddType(IServiceCollection services, Type type);

    protected virtual void TriggerServiceExposingActions(IServiceCollection services, Type implementationType,
        List<Type> exposedServiceTypes)
    {
        var actions = services.GetServiceExposingActionList();
        
        if (actions.Count <= 0) return;
        
        var context = new OnServiceExposingContext(implementationType, exposedServiceTypes);
        foreach (var action in actions)
        {
            action.Invoke(context);
        }
    }

    protected virtual List<Type> GetExposedServiceTypes(Type type)
    {
        var defaultExposeServicesAttribute =
            new ExposeServicesAttribute
            {
                ExposeConventionalInterfaces = true,
                ExposeSelf = true
            };
        return type
            .GetCustomAttributes(true)
            .OfType<IExposedServiceTypesProvider>()
            .DefaultIfEmpty(defaultExposeServicesAttribute)
            .SelectMany(p => p.GetExposedServiceTypes(type))
            .Distinct()
            .ToList();
    }
    
    protected virtual ServiceLifetime? GetLifeTimeOrNull(Type type, [CanBeNull] DependencyAttribute attribute)
    {
        // 优先从Attribute读取，其次是类的层次体系
        return attribute?.Lifetime ?? GetServiceLifetimeFromClassHierarchy(type);
    }
    
    
    /// <summary>
    /// 从类的层次体系中提取生命周期
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    protected virtual ServiceLifetime? GetServiceLifetimeFromClassHierarchy(Type type)
    {
        if (typeof(ISingletonDependency).IsAssignableFrom(type))
        {
            return ServiceLifetime.Singleton;
        }
        
        if (typeof(IScopedDependency).IsAssignableFrom(type))
        {
            return ServiceLifetime.Scoped;
        }
        
        if (typeof(ITransientDependency).IsAssignableFrom(type))
        {
            return ServiceLifetime.Transient;
        }

        return null;
    }
}