namespace Fax.Core.DependencyInjection;

public enum ServiceLifecycle
{
    /// <summary>
    /// 瞬态
    /// </summary>
    Transient,
    /// <summary>
    /// 局域
    /// </summary>
    Scoped,
    /// <summary>
    /// 单例
    /// </summary>
    Singleton
}