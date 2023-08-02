namespace Fake.Reflection;

public interface ITypeScanner
{
    IReadOnlyList<Type> Scan();
}