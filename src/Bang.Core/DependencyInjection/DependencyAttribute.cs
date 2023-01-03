namespace Bang.Core.DependencyInjection;

/// <summary>
/// 依赖注入的配置
/// </summary>
public class DependencyAttribute : Attribute
{
    /// <summary>
    /// 当重复注册服务依赖时。
    /// true代表：使用IServiceCollection的Replace，替换之前已经注册过的依赖。
    /// false代表：使用IServiceCollection的TryAdd，放弃本次注册。
    /// </summary>
    public bool Replace { get; set; }

    /// <summary>
    /// 服务的生命周期。
    /// 如果未定义DependencyAttribute，取实现的最后一个生命周期。
    /// </summary>
    public ServiceLifecycle Lifecycle { get; set; }

    #region ctor

    public DependencyAttribute() : this(ServiceLifecycle.Transient)
    {
    }

    public DependencyAttribute(ServiceLifecycle lifecycle, bool replaceService = true)
    {
        Lifecycle = lifecycle;
        Replace = replaceService;
    }

    #endregion
}