namespace Bang.Core.DependencyInjection;

public enum ServiceLifecycle
{
    /// <summary>
    /// 瞬态
    /// </summary>
    Transient,
    /// <summary>
    /// 单次请求单例
    /// </summary>
    Scoped,
    /// <summary>
    /// 单例
    /// </summary>
    Singleton
}