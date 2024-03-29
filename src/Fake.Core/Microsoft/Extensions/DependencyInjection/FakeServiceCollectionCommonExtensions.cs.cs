﻿using Fake;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeServiceCollectionCommonExtensions
{
    public static bool IsAdded<T>(this IServiceCollection services)
    {
        return services.IsAdded(typeof(T));
    }

    public static bool IsAdded(this IServiceCollection services, Type type)
    {
        return services.Any(d => d.ServiceType == type);
    }


    public static void EnsureAdded<T>(this IServiceCollection services) where T : class
    {
        if (!services.IsAdded<T>())
        {
            throw new FakeInitializationException("请先添加服务：" + typeof(T).AssemblyQualifiedName);
        }
    }

    /// <summary>
    /// 直接从IOC容器中获取实例，找不到就返回null。
    /// tips：仅仅是简单的根据type取ImplementationInstance，尽可能的从ServiceProvider取而不是ServiceCollection
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? GetInstanceOrNull<T>(this IServiceCollection services) where T : class
    {
        return services
            .FirstOrDefault(d => d.ServiceType == typeof(T))
            ?.ImplementationInstance?.To<T>();
    }

    /// <summary>
    /// 直接从IOC容器中获取实例。
    /// tips：仅仅是简单的根据type取ImplementationInstance，尽可能的从ServiceProvider取而不是ServiceCollection
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">找不到服务实例</exception>
    public static T GetInstance<T>(this IServiceCollection services) where T : class
    {
        var service = services.GetInstanceOrNull<T>();
        if (service == null)
        {
            throw new InvalidOperationException($"找不到服务实例：{typeof(T).AssemblyQualifiedName}");
        }

        return service;
    }

    public static IServiceProvider BuildServiceProviderFromFactory(this IServiceCollection services)
    {
        ThrowHelper.ThrowIfNull(services, nameof(services));

        var factoryInterfaceType = services.FirstOrDefault(x =>
            x.ImplementationInstance?.GetType().GetInterfaces()
                .Where(t => t.IsGenericType)
                .Select(t => t.GetGenericTypeDefinition())
                .Contains(typeof(IServiceProviderFactory<>)
                ) ?? false)?.ServiceType;

        if (factoryInterfaceType == null)
        {
            // 如果没有第三方IOC容器则使用默认的
            return services.BuildServiceProvider();
        }

        // 通过服务商工厂创建服务商
        var containerBuilderType = factoryInterfaceType.GenericTypeArguments[0];
        var serviceProvider = typeof(FakeServiceCollectionCommonExtensions)
            .GetMethods()
            .Single(m => m.Name == nameof(BuildServiceProviderFromFactory) && m.IsGenericMethod)
            .MakeGenericMethod(containerBuilderType)
            .Invoke(null, new object[] { services, null! });

        return serviceProvider.To<IServiceProvider>();
    }

    public static IServiceProvider BuildServiceProviderFromFactory<TContainerBuilder>(
        this ServiceCollection services, Action<TContainerBuilder>? containerBuildAction = null)
        where TContainerBuilder : notnull
    {
        ThrowHelper.ThrowIfNull(services, nameof(services));


        // 这里默认是拿第一个实现，也就是第一个工厂
        // 一般来说我们要不就是使用默认的service provider，即aspnetcore通过BuildServiceProvider new出来的那个
        // 要不就是用类似autofac这种第三方provider，一个项目一般不会同时出现两个第三方service provider
        var serviceProviderFactory = services.GetInstanceOrNull<IServiceProviderFactory<TContainerBuilder>>();
        if (serviceProviderFactory == null)
        {
            throw new FakeException($"找不到{typeof(IServiceProviderFactory<TContainerBuilder>).FullName}的实现");
        }

        var builder = serviceProviderFactory.CreateBuilder(services);
        containerBuildAction?.Invoke(builder);

        return serviceProviderFactory.CreateServiceProvider(builder);
    }
}