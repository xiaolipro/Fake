namespace Fake.DependencyInjection;

public class ObjectAccessor<T>
{
    [CanBeNull]
    public T Value { get; set; }

    public ObjectAccessor()
    {

    }

    public ObjectAccessor([CanBeNull] T obj)
    {
        Value = obj;
    }
}