using Fake;
using Fake.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeObjectAccessorServiceCollectionExtensions
{
    public static ObjectAccessor<T> GetOrAddObjectAccessor<T>(this IServiceCollection services)
    {
        if (services.Any(s => s.ServiceType == typeof(ObjectAccessor<T>)))
        {
            return services.GetInstance<ObjectAccessor<T>>();
        }

        return services.AddObjectAccessor<T>();
    }

    public static ObjectAccessor<T>? GetObjectAccessorOrNull<T>(this IServiceCollection services)
        where T : class
    {
        return services.GetInstanceOrNull<ObjectAccessor<T>>();
    }

    public static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services)
    {
        return services.AddObjectAccessor(new ObjectAccessor<T>());
    }

    public static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services, T obj)
    {
        return services.AddObjectAccessor(new ObjectAccessor<T>(obj));
    }

    public static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services, ObjectAccessor<T> accessor)
    {
        if (services.Any(s => s.ServiceType == typeof(ObjectAccessor<T>)))
        {
            throw new FakeException($"已经注册过: {typeof(T).AssemblyQualifiedName} 的 {nameof(ObjectAccessor<T>)} 了");
        }

        //Add to the beginning for fast retrieve
        services.Insert(0, ServiceDescriptor.Singleton(typeof(ObjectAccessor<T>), accessor));

        return accessor;
    }
}