namespace Fake.Modularity;

public interface IDependsOnProvider
{
    Type[] GetDependedTypes();
}