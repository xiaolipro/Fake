using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Fake.Autofac;
using Fake.Castle.DynamicProxy;
using Fake.DependencyInjection;
using Fake.Modularity;

namespace Autofac.Builder;

public static class FakeRegistrationBuilderExtensions
{
    public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> ConfigureFakeConventions<
        TLimit, TActivatorData, TRegistrationStyle>(
        this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder,
        IModuleContainer moduleContainer,
        ServiceRegistrationActionList registrationActionList)
    {
        var serviceType = registrationBuilder.RegistrationData.Services
            .OfType<IServiceWithType>()
            .FirstOrDefault()?.ServiceType;

        if (serviceType == null) return registrationBuilder;

        Type implementationType = null;

        if (registrationBuilder.ActivatorData is ReflectionActivatorData reflectionActivatorData)
        {
            implementationType = reflectionActivatorData.ImplementationType;
        }
        else if (registrationBuilder.ActivatorData is SimpleActivatorData simpleActivatorData)
        {
            implementationType = simpleActivatorData.GetType();
        }

        Debug.Assert(implementationType != null, nameof(implementationType) + " != null");
        return registrationBuilder
            .EnablePropertyInjection(moduleContainer, implementationType)  //启用属性注入
            .InvokeRegistrationActions(registrationActionList, serviceType, implementationType); 
    }

    private static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> EnablePropertyInjection<TLimit,
        TActivatorData, TRegistrationStyle>(
        this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder,
        IModuleContainer moduleContainer,
        Type implementationType)
    {
        // 只对Fake模块中没有使用DisablePropertyInjectionAttribute的实现开启属性注入
        if (moduleContainer.Modules.Any(m => m.Assembly == implementationType.Assembly) &&
            implementationType.GetCustomAttributes(typeof(DisablePropertyInjectionAttribute), true).IsNullOrEmpty())
        {
            // preserveSetValues设为false，不保留原有值，覆写
            registrationBuilder = registrationBuilder.PropertiesAutowired(new FakePropertySelector(false));
        }

        return registrationBuilder;
    }

    private static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InvokeRegistrationActions<TLimit,
        TActivatorData, TRegistrationStyle>(
        this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder,
        ServiceRegistrationActionList registrationActionList,
        Type serviceType,
        Type implementationType)
    {
        var context = new OnServiceRegistrationContext(serviceType, implementationType);

        foreach (var registrationAction in registrationActionList)
        {
            registrationAction.Invoke(context);
        }

        if (context.Interceptors.Any())
        {
            registrationBuilder = registrationBuilder.AddInterceptors(
                registrationActionList,
                serviceType,
                context.Interceptors
            );
        }

        return registrationBuilder;
    }


    private static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle>
        AddInterceptors<TLimit, TActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder,
            ServiceRegistrationActionList serviceRegistrationActionList,
            Type serviceType,
            IEnumerable<Type> interceptors)
    {
        if (serviceType.IsInterface)
        {
            registrationBuilder.EnableInterfaceInterceptors();
        }
        else
        {
            if (serviceRegistrationActionList.DisableClassInterceptors) return registrationBuilder;

            (registrationBuilder as IRegistrationBuilder<TLimit, ConcreteReflectionActivatorData, TRegistrationStyle>)
                .EnableClassInterceptors();
        }

        foreach (var interceptor in interceptors)
        {
            // 使用Castle做动态代理
            registrationBuilder.InterceptedBy(typeof(FakeAsyncDeterminationInterceptor<>).MakeGenericType(interceptor));
        }

        return registrationBuilder;
    }
}