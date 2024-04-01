using System.Reflection;
using Fake.Helpers;

namespace Fake.DependencyInjection;

public abstract class AbstractServiceRegistrar : IServiceRegistrar
{
    public virtual void RegisterAssembly(IServiceCollection services, Assembly assembly)
    {
        var types = ReflectionHelper
            .GetAssemblyAllTypes(assembly)
            .Where(
                //TODO：泛型为什么跳过？
                // type => type is { IsClass: true, IsAbstract: false }
                type => type is { IsClass: true, IsAbstract: false, IsGenericType: false }
            ).ToArray();

        RegisterTypes(services, types);
    }

    public virtual void RegisterTypes(IServiceCollection services, params Type[] types)
    {
        foreach (var type in types)
        {
            RegisterType(services, type);
        }
    }

    public abstract void RegisterType(IServiceCollection services, Type type);

    protected virtual void TriggerServiceExposingActions(IServiceCollection services, Type implementationType,
        List<ServiceIdentifier> exposedServices)
    {
        var actions = services.GetServiceExposingActionList();

        if (actions.Count <= 0) return;

        var context = new OnServiceExposingContext(implementationType, exposedServices);
        foreach (var action in actions)
        {
            action.Invoke(context);
        }
    }

    /// <summary>
    /// 是否跳过服务注册
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    protected virtual bool IsSkipServiceRegistration(Type type)
    {
        return type.IsDefined(typeof(DisableServiceRegistrationAttribute), true);
    }

    /// <summary>
    /// 获取服务生命周期：优先从Attribute读取，其次是类的层次体系
    /// </summary>
    /// <param name="type">给定类型</param>
    /// <param name="attribute">依赖注入的配置</param>
    /// <returns></returns>
    protected virtual ServiceLifetime? GetLifeTimeOrNull(Type type, DependencyAttribute? attribute)
    {
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