namespace Bang.DependencyInjection;

/// <summary>
/// 依赖注入的配置
/// </summary>
public class ServiceRegisterAttribute : Attribute
{
    /// <summary>
    /// 设置true则替换之前已经注册过的服务.使用IServiceCollection的Replace扩展方法.
    /// 设置false则只注册以前未注册的服务.使用IServiceCollection的TryAdd扩展方法.
    /// </summary>
    public bool Replace { get; set; }

    /// <summary>
    /// 生命周期:Singleton,Transient或Scoped.
    /// </summary>
    public ServiceLifetime Lifetime { get; set; }

    public ServiceRegisterAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }
}