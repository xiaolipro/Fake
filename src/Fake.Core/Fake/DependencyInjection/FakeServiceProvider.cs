using System.Collections.Concurrent;

namespace Fake.DependencyInjection;

public class FakeServiceProvider : IFakeServiceProvider
{
    protected IServiceProvider ServiceProvider { get; }
    protected ConcurrentDictionary<Type, Lazy<object>> ServiceCacheDic { get; }

    public FakeServiceProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        ServiceCacheDic = new ConcurrentDictionary<Type, Lazy<object>>();
        ServiceCacheDic.TryAdd(typeof(IServiceProvider), new Lazy<object>(() => ServiceProvider));
    }

    public T GetLazyService<T>(Func<IServiceProvider, object> valueFactory = null) where T: class
    {
        return GetLazyService(typeof(T), valueFactory) as T;
    }

    public object GetLazyService(Type serviceType, Func<IServiceProvider, object> valueFactory = null)
    {
        if (valueFactory != null)
            return ServiceCacheDic.GetOrAdd(
                serviceType,
                _ => new Lazy<object>(() => valueFactory(ServiceProvider))
            ).Value;
        
        return ServiceCacheDic.GetOrAdd(
            serviceType,
            _ => new Lazy<object>(() => ServiceProvider.GetService(serviceType))
        ).Value;
    }

    public T GetRequiredLazyService<T>() where T: class
    {
        return GetRequiredLazyService(typeof(T)) as T;
    }

    public object GetRequiredLazyService(Type serviceType)
    {
        return ServiceCacheDic.GetOrAdd(
            serviceType,
            _ => new Lazy<object>(() => ServiceProvider.GetRequiredService(serviceType))
        ).Value;
    }
}