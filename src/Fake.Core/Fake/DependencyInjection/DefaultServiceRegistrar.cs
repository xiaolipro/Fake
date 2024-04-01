using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fake.DependencyInjection;

public class DefaultServiceRegistrar : AbstractServiceRegistrar
{
    public override void RegisterType(IServiceCollection services, Type type)
    {
        if (IsSkipServiceRegistration(type)) return;

        var attribute = type.GetCustomAttribute<DependencyAttribute>(true);

        // 获取服务生命周期
        var lifetime = GetLifeTimeOrNull(type, attribute);
        if (lifetime == null) return;

        // 获取需要暴露的服务
        var exposedServices = GetExposedServices(type);

        // 触发服务暴露动作
        TriggerServiceExposingActions(services, type, exposedServices);

        var allExposedServiceTypes = exposedServices.Select(x => x.ServiceType).ToList();

        foreach (var exposedService in exposedServices)
        {
            var serviceDescriptor =
                CreateServiceDescriptor(type, exposedService, allExposedServiceTypes, lifetime.Value);

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

    protected virtual List<ServiceIdentifier> GetExposedServices(Type type)
    {
        return ExposedServiceExplorer.GetExposedServices(type);
    }

    protected virtual ServiceDescriptor CreateServiceDescriptor(Type implementationType,
        ServiceIdentifier exposedService,
        List<Type> allExposedServiceTypes, ServiceLifetime lifetime)
    {
        // fast path
        if (lifetime == ServiceLifetime.Transient)
            return exposedService.ServiceKey == null
                ? ServiceDescriptor.Describe(exposedService.ServiceType, implementationType, lifetime)
                : ServiceDescriptor.DescribeKeyed(exposedService.ServiceType, exposedService.ServiceKey
                    , implementationType, lifetime);

        // scope/singleton 服务需要特殊处理，适配类的层次体系
        var redirectedType = GetRedirectedTypeOrNull(
            implementationType,
            exposedService.ServiceType,
            allExposedServiceTypes
        );

        if (redirectedType != null)
        {
            return exposedService.ServiceKey == null
                ? ServiceDescriptor.Describe(exposedService.ServiceType
                    , sp => sp.GetRequiredService(redirectedType)
                    , lifetime)
                : ServiceDescriptor.DescribeKeyed(exposedService.ServiceType
                    , exposedService.ServiceKey
                    , (sp, key) => sp.GetRequiredKeyedService(redirectedType, key)
                    , lifetime);
        }

        return exposedService.ServiceKey == null
            ? ServiceDescriptor.Describe(exposedService.ServiceType, implementationType, lifetime)
            : ServiceDescriptor.DescribeKeyed(exposedService.ServiceType, exposedService.ServiceKey
                , implementationType, lifetime);
    }

    protected virtual Type? GetRedirectedTypeOrNull(Type implementationType, Type exposedServiceType,
        List<Type> allExposedServiceTypes)
    {
        // 暴露的服务数量小于2代表没有形成层次体系，不需要重定向
        if (allExposedServiceTypes.Count < 2) return null;

        // 如果暴露的实现本身，不需要重定向
        if (exposedServiceType == implementationType) return null;

        // 如果实现暴露了，则所有暴露都应该重定向到实现
        if (allExposedServiceTypes.Contains(implementationType))
        {
            return implementationType;
        }

        // 重定向到第一个可分配的暴露
        return allExposedServiceTypes.FirstOrDefault(x =>
            x != exposedServiceType && x.IsAssignableTo(exposedServiceType));
    }
}