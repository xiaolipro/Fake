namespace Fake.Proxy;

public class DynamicProxyIgnoreTypes
{
    private static readonly HashSet<Type> IgnoredTypes = new();

    public static void Add<TType>()
    {
        lock (IgnoredTypes)
        {
            IgnoredTypes.TryAdd(typeof(TType));
        }
    }


    public static bool Contains(Type type, bool includeDerivedTypes = true)
    {
        lock (IgnoredTypes)
        {
            return includeDerivedTypes ? IgnoredTypes.Any(type.IsAssignableTo): IgnoredTypes.Contains(type);
        }
    }
}