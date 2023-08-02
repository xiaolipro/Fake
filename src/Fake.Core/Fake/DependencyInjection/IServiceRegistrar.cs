using System.Reflection;

namespace Fake.DependencyInjection;

public interface IServiceRegistrar
{
    /// <summary>
    /// 注册程序集内所有需要注册的服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    void RegisterAssembly(IServiceCollection services, Assembly assembly);
    
    /// <summary>
    /// 注册指定类型集合所有需要注册的服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="types"></param>
    void RegisterTypes(IServiceCollection services, params Type[] types);
    
    /// <summary>
    /// 注册指定类型所有需要注册的服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="type"></param>
    void RegisterType(IServiceCollection services, Type type);
}