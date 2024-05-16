using System.Reflection;
using Fake.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeServiceRegisterServiceCollectionExtensions
{
    #region ServiceRegistered 服务注册后切面

    /// <summary>
    /// 在每个服务注册到IOC容器后调度
    /// </summary>
    /// <param name="services"></param>
    /// <param name="registrationAction"></param>
    public static void OnRegistered(this IServiceCollection services,
        Action<OnServiceRegistrationContext> registrationAction)
    {
        GetOrCreateRegistrationActionList(services).Add(registrationAction);
    }

    public static ServiceRegistrationActionList GetRegistrationActionList(this IServiceCollection services)
    {
        return GetOrCreateRegistrationActionList(services);
    }

    private static ServiceRegistrationActionList GetOrCreateRegistrationActionList(IServiceCollection services)
    {
        var actionList = services.GetObjectAccessorOrNull<ServiceRegistrationActionList>()?.Value;
        if (actionList == null)
        {
            actionList = new ServiceRegistrationActionList();
            services.AddObjectAccessor(actionList);
        }

        return actionList;
    }

    /// <summary>
    /// 禁用类的拦截器，接口拦截器不受影响
    /// </summary>
    /// <param name="services"></param>
    public static void DisableClassInterceptors(this IServiceCollection services)
    {
        GetOrCreateRegistrationActionList(services).DisableClassInterceptors = true;
    }

    #endregion

    #region ServiceExposing 服务暴露切面

    /// <summary>
    /// 服务暴露时调度，可以在这里变更暴露内容
    /// </summary>
    /// <param name="services"></param>
    /// <param name="exposeAction"></param>
    public static void OnServiceExposing(this IServiceCollection services,
        Action<OnServiceExposingContext> exposeAction)
    {
        GetOrCreateExposingList(services).Add(exposeAction);
    }

    public static ServiceExposingActionList GetServiceExposingActionList(this IServiceCollection services)
    {
        return GetOrCreateExposingList(services);
    }

    private static ServiceExposingActionList GetOrCreateExposingList(IServiceCollection services)
    {
        var actionList = services.GetObjectAccessorOrNull<ServiceExposingActionList>()?.Value;
        if (actionList == null)
        {
            actionList = new ServiceExposingActionList();
            services.AddObjectAccessor(actionList);
        }

        return actionList;
    }

    #endregion

    #region ServiceRegistrar 服务注册切面

    /// <summary>
    /// 注册给定程序集内所有满足注册标准的服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    internal static IServiceCollection RegisterAssembly(this IServiceCollection services, Assembly assembly)
    {
        foreach (var registrar in services.GetOrCreateServiceRegisterList())
        {
            registrar.RegisterAssembly(services, assembly);
        }

        return services;
    }

    /// <summary>
    /// 添加服务注册器，服务注册时会执行每一个注册器
    /// </summary>
    /// <param name="services"></param>
    /// <param name="registrar"></param>
    /// <returns></returns>
    public static IServiceCollection AddServiceRegistrar(this IServiceCollection services, IServiceRegistrar registrar)
    {
        services.GetOrCreateServiceRegisterList().Add(registrar);

        return services;
    }


    /// <summary>
    /// 获取或创建服务注册器集合，服务注册时会执行每一个注册器
    /// </summary>
    /// <param name="services"></param>
    /// <returns>服务注册器列表</returns>
    private static ServiceRegistrarList GetOrCreateServiceRegisterList(this IServiceCollection services)
    {
        var conventionalRegistrars = services.GetObjectAccessorOrNull<ServiceRegistrarList>()?.Value;
        if (conventionalRegistrars == null)
        {
            conventionalRegistrars = new ServiceRegistrarList();
            conventionalRegistrars.Add(new DefaultServiceRegistrar());
            services.AddObjectAccessor(conventionalRegistrars);
        }

        return conventionalRegistrars;
    }


    public static IServiceCollection RegisterType<TType>(this IServiceCollection services)
    {
        return services.RegisterType(typeof(TType));
    }

    public static IServiceCollection RegisterType(this IServiceCollection services, Type type)
    {
        foreach (var registrar in services.GetOrCreateServiceRegisterList())
        {
            registrar.RegisterType(services, type);
        }

        return services;
    }

    #endregion
}