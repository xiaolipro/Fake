namespace Fake.Logging;

public class DefaultFakeLoggerFactory:IFakeLoggerFactory
{
    private readonly Dictionary<Type, object> _cache = new();
    public virtual IFakeLogger<T> Create<T>()
    {
        return _cache.GetOrAdd(typeof(T), () => new DefaultFakeLogger<T>()) as IFakeLogger<T>;
    }
}