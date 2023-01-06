using Bang.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class BangServiceCollectionObjectAccessorExtensions
{
    public static IObjectAccessor<T> TryAddObjectAccessor<T>(this IServiceCollection services)
    {
        if (services.Any(s => s.ServiceType == typeof(IObjectAccessor<T>)))
        {
            return services.GetSingletonInstance<IObjectAccessor<T>>();
        }

        return services.AddObjectAccessor<T>();
    }
    
    public static IObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services)
    {
        return services.AddObjectAccessor(new ObjectAccessor<T>());
    }
    
    public static IObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services, T obj)
    {
        return services.AddObjectAccessor(new ObjectAccessor<T>(obj));
    }
    
    public static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services, ObjectAccessor<T> accessor)
    {
        if (services.Any(s => s.ServiceType == typeof(IObjectAccessor<T>)))
        {
            throw new Exception("An object accessor is registered before for type: " + typeof(T).AssemblyQualifiedName);
        }

        //Add to the beginning for fast retrieve
        services.Insert(0, ServiceDescriptor.Singleton(typeof(IObjectAccessor<T>), accessor));

        return accessor;
    }
}