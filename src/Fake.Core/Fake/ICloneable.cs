namespace Fake;

// ReSharper disable once TypeParameterCanBeVariant
public interface ICloneable<T> where T:class
{
    T Clone();
}