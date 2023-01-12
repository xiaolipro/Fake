namespace Fake.Logging;

public class FakeInitLoggerFactory:IInitLoggerFactory
{
    private readonly Dictionary<Type, object> _cache = new();
    public virtual IInitLogger<T> Create<T>()
    {
        return _cache.GetOrAdd(typeof(T), () => new FakeInitLogger<T>()) as IInitLogger<T>;
    }
}