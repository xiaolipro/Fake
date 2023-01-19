namespace Fake.Collections;

public class TypeList<TBaseType> :List<TBaseType>,ITypeList<TBaseType>
{
    public IEnumerator<Type> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public void Add(Type item)
    {
        throw new NotImplementedException();
    }

    public bool Contains(Type item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(Type[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(Type item)
    {
        throw new NotImplementedException();
    }

    public bool IsReadOnly { get; }
    public int IndexOf(Type item)
    {
        throw new NotImplementedException();
    }

    public void Insert(int index, Type item)
    {
        throw new NotImplementedException();
    }

    public Type this[int index]
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public void Add<T>() where T : TBaseType
    {
        throw new NotImplementedException();
    }

    public bool TryAdd<T>() where T : TBaseType
    {
        throw new NotImplementedException();
    }

    public bool Contains<T>() where T : TBaseType
    {
        throw new NotImplementedException();
    }

    public void Remove<T>() where T : TBaseType
    {
        throw new NotImplementedException();
    }
}