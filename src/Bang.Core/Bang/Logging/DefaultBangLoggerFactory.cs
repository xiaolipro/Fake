namespace Bang.Logging;

public class DefaultBangLoggerFactory:IBangLoggerFactory
{
    private readonly Dictionary<Type, object> _cache = new();
    public virtual IBangLogger<T> Create<T>()
    {
        return _cache.GetOrAdd(typeof(T), () => new DefaultBangLogger<T>()) as IBangLogger<T>;
    }
}