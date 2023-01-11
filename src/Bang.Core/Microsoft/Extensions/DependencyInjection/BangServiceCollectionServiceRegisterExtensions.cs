using System.Reflection;
using Bang.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class BangServiceCollectionServiceRegisterExtensions
{
    #region Exposing
    /// <summary>
    /// 服务暴露时执行
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

    #region ServiceRegister
    public static IServiceCollection AddAssembly(this IServiceCollection services, Assembly assembly)
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
    private static ServiceRegisterList GetOrCreateServiceRegisterList(this IServiceCollection services)
    {
        var conventionalRegistrars = services.GetObjectAccessorOrNull<ServiceRegisterList>()?.Value;
        if (conventionalRegistrars == null)
        {
            conventionalRegistrars = new ServiceRegisterList();
            services.AddObjectAccessor(conventionalRegistrars);
        }

        return conventionalRegistrars;
    }

    #endregion
}