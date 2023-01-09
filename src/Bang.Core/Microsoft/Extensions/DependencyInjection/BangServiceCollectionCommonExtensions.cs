namespace Microsoft.Extensions.DependencyInjection;

public static class BangServiceCollectionCommonExtensions
{
    public static bool IsAdded<T>(this ServiceCollection services)
    {
        return services.IsAdded(typeof(T));
    }
    
    public static bool IsAdded(this ServiceCollection services, Type type)
    {
        return services.Any(x => x.ServiceType == type);
    }
    public static T GetSingletonInstanceOrNull<T>(this IServiceCollection services)
    {
        return (T)services
            .FirstOrDefault(d => d.ServiceType == typeof(T))
            ?.ImplementationInstance;
    }
    
    public static T GetSingletonInstance<T>(this IServiceCollection services)
    {
        var service = services.GetSingletonInstanceOrNull<T>();
        if (service == null)
        {
            throw new InvalidOperationException("找不到 singleton service: " + typeof(T).AssemblyQualifiedName);
        }

        return service;
    }
}