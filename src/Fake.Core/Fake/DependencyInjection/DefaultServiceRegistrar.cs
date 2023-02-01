using System.Reflection;
using Fake.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fake.DependencyInjection;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class DefaultServiceRegistrar : AbstractServiceRegistrar
{
    public override void AddType(IServiceCollection services, Type type)
    {
        if (IsDisableServiceRegistration(type)) return;

        var attribute = type.GetCustomAttribute<DependencyAttribute>(true);

        // 获取服务生命周期
        var lifetime = base.GetLifeTimeOrNull(type, attribute);
        if (lifetime == null) return;

        // 获取需要暴露的服务
        var exposedServiceTypes = ExposedServiceExplorer.GetExposedServiceTypes(type);

        // 触发服务暴露动作
        TriggerServiceExposingActions(services, type, exposedServiceTypes);

        foreach (var exposedServiceType in exposedServiceTypes)
        {
            var serviceDescriptor = CreateServiceDescriptor(
                type,
                exposedServiceType,
                exposedServiceTypes,
                lifetime.Value
            );

            if (attribute?.Replace == true)
            {
                services.Replace(serviceDescriptor);
            }
            else if (attribute?.TryAdd == true)
            {
                services.TryAdd(serviceDescriptor);
            }
            else
            {
                services.Add(serviceDescriptor);
            }
        }
    }

    protected virtual ServiceDescriptor CreateServiceDescriptor(Type implementationType, Type exposedServiceType,
        List<Type> allExposedServiceTypes, ServiceLifetime lifetime)
    {
        if (!lifetime.IsIn(ServiceLifetime.Singleton, ServiceLifetime.Scoped))
            return ServiceDescriptor.Describe(
                exposedServiceType,
                implementationType,
                lifetime
            );
        
        // 适配类的层次体系
        var redirectedType = GetRedirectedTypeOrNull(
            implementationType,
            exposedServiceType,
            allExposedServiceTypes
        );

        if (redirectedType != null)
        {
            return ServiceDescriptor.Describe(
                exposedServiceType,
                provider => provider.GetRequiredService(redirectedType),
                lifetime
            );
        }
        
        return ServiceDescriptor.Describe(
            exposedServiceType,
            implementationType,
            lifetime
        );
    }

    protected virtual Type GetRedirectedTypeOrNull(Type implementationType, Type exposedServiceType,
        List<Type> allExposedServiceTypes)
    {
        // 暴露的服务数量小于2代表没有形成层次体系，不需要转发
        if (allExposedServiceTypes.Count < 2) return null;
        
        // 如果实现自爆，不需要转发
        if (exposedServiceType == implementationType) return null;

        // 如果实现自爆了，则所有暴露都重定向到实现
        if (allExposedServiceTypes.Contains(implementationType))
        {
            return implementationType;
        }

        // 重定向到可分配自的暴露
        return allExposedServiceTypes.FirstOrDefault(x =>
            x != exposedServiceType && x.IsAssignableTo(exposedServiceType));
    }

    protected virtual bool IsDisableServiceRegistration(Type type)
    {
        return type.IsDefined(typeof(DisableServiceRegistrationAttribute), true);
    }
}