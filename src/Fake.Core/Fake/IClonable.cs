namespace Fake;

// ReSharper disable once TypeParameterCanBeVariant
public interface IClonable<T> where T:class
{
    T Clone();
}