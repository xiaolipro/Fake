using System.Reflection;
using Fake.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeServiceCollectionServiceRegisterExtensions
{
    #region ServiceRegistered

    /// <summary>
    /// 在每个服务注册到IOC容器后执行
    /// </summary>
    /// <param name="services"></param>
    /// <param name="registrationAction"></param>
    public static void OnRegistered(this IServiceCollection services, Action<OnServiceRegistrationContext> registrationAction)
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
    #endregion
    
    #region ServiceExposing
    
    /// <summary>
    /// 服务暴露时执行，可以在这里变更暴露内容
    /// </summary>
    /// <param name="services"></param>
    /// <param name="exposeAction"></param>
    public static void OnServiceExposing(this IServiceCollection services, Action<OnServiceExposingContext> exposeAction)
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

    #region ServiceRegistrar
    internal static IServiceCollection AddAssembly(this IServiceCollection services, Assembly assembly)
    {
        foreach (var registrar in services.GetOrCreateServiceRegisterList())
        {
            registrar.AddAssembly(services, assembly);
        }

        return services;
    }
    
    public static IServiceCollection AddServiceRegistrar(this IServiceCollection services, IServiceRegistrar registrar)
    {
        services.GetOrCreateServiceRegisterList().Add(registrar);

        return services;
    }
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

    #endregion
}