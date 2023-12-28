namespace Fake.DependencyInjection;

/// <summary>
/// Lazy服务商
/// </summary>
public interface ILazyServiceProvider
{
    /// <summary>
    /// 从Lazy服务商获取服务
    /// </summary>
    /// <param name="valueFactory">如果服务不存在，则构建</param>
    /// <typeparam name="T">服务类型</typeparam>
    /// <returns></returns>
    T? GetService<T>(Func<IServiceProvider, object>? valueFactory = null) where T : class;

    /// <summary>
    /// 从Lazy服务商获取服务
    /// </summary>
    /// <param name="serviceType">服务类型</param>
    /// <param name="valueFactory">如果服务不存在，则构建</param>
    /// <returns></returns>
    object? GetService(Type serviceType, Func<IServiceProvider, object>? valueFactory = null);

    /// <summary>
    /// 从Lazy服务商获取服务，如果服务不存在则抛出异常
    /// </summary>
    /// <typeparam name="T">服务类型</typeparam>
    /// <returns></returns>
    T GetRequiredService<T>() where T : class;

    /// <summary>
    /// 从Lazy服务商获取服务，如果服务不存在则抛出异常
    /// </summary>
    /// <returns></returns>
    /// <exception cref="T:System.InvalidOperationException">There is no service of type <paramref name="serviceType" />.</exception>
    object GetRequiredService(Type serviceType);
}