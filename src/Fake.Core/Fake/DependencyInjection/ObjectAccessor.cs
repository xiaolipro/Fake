namespace Fake.DependencyInjection;

/// <summary>
/// 泛型单例访问器
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectAccessor<T>
{
    public T? Value { get; set; }

    public ObjectAccessor()
    {
    }

    public ObjectAccessor(T obj)
    {
        Value = obj;
    }
}