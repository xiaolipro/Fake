using System.Collections.Concurrent;

namespace Fake.DependencyInjection;

/// <summary>
/// 高效的Lazy服务供应商
/// </summary>
public class LazyServiceProvider : ILazyServiceProvider
{
    protected IServiceProvider ServiceProvider { get; }
    protected ConcurrentDictionary<Type, Lazy<object?>> ServiceCacheDic { get; }

    public LazyServiceProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        ServiceCacheDic = new ConcurrentDictionary<Type, Lazy<object?>>();
        ServiceCacheDic.TryAdd(typeof(IServiceProvider), new Lazy<object?>(() => serviceProvider));
    }

    public T? GetService<T>(Func<IServiceProvider, object>? valueFactory = null) where T : class
    {
        return GetService(typeof(T), valueFactory) as T;
    }

    public object? GetService(Type serviceType, Func<IServiceProvider, object>? valueFactory = null)
    {
        if (valueFactory != null)
            return ServiceCacheDic.GetOrAdd(
                serviceType,
                _ => new Lazy<object?>(() => valueFactory(ServiceProvider))
            ).Value;

        return ServiceCacheDic.GetOrAdd(
            serviceType,
            _ => new Lazy<object?>(() => ServiceProvider.GetService(serviceType))
        ).Value;
    }

    public T GetRequiredService<T>() where T : class
    {
        return GetRequiredService(typeof(T)).To<T>();
    }

    public object GetRequiredService(Type serviceType)
    {
        return ServiceCacheDic.GetOrAdd(
            serviceType,
            _ => new Lazy<object?>(() => ServiceProvider.GetRequiredService(serviceType))
        ).Value!;
    }
}