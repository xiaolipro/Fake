using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bang.DependencyInjection;

public class DefaultServiceRegistrar:AbstractServiceRegistrar
{
    public override void AddType(IServiceCollection services, Type type)
    {
        if (IsDisableServiceRegistration(type)) return;

        var serviceRegisterAttribute = type.GetCustomAttribute<ServiceRegisterAttribute>(true);
        
        // 获取服务生命周期
        var lifetime = base.GetLifeTimeOrNull(type, serviceRegisterAttribute);
        if (lifetime == null) return;
        
        // 获取需要暴露的服务
        var exposedServiceTypes = base.GetExposedServiceTypes(type);

        // 触发服务暴露动作
        TriggerServiceExposingActions(services, type, exposedServiceTypes);

        foreach (var exposedServiceType in exposedServiceTypes)
        {
            var serviceDescriptor = ServiceDescriptor.Describe(
                exposedServiceType,
                type,
                lifetime.Value
            );

            if (serviceRegisterAttribute?.Replace == true)
            {
                services.Replace(serviceDescriptor);
            }
            else
            {
                services.TryAdd(serviceDescriptor);
            }
        }
       
    }

    protected virtual bool IsDisableServiceRegistration(Type type)
    {
        return type.IsDefined(typeof(DisableServiceRegistrationAttribute), true);
    }
}