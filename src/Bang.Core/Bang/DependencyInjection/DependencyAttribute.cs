namespace Bang.DependencyInjection;

/// <summary>
/// 依赖注入的配置
/// </summary>
public class DependencyAttribute : Attribute
{
    /// <summary>
    /// 替换之前已经注册过的服务，使用IServiceCollection的Replace扩展方法
    /// </summary>
    public bool Replace { get; set; }

    /// <summary>
    /// 只注册以前未注册的服务，使用IServiceCollection的TryAdd扩展方法
    /// </summary>
    public bool TryAdd { get; set; }

    /// <summary>
    /// 生命周期:Singleton，Transient或Scoped
    /// </summary>
    public ServiceLifetime? Lifetime { get; set; }

    public DependencyAttribute()
    {
        
    }

    public DependencyAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }
}