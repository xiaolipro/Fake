namespace Fake.Modularity;

public interface IDependsOnProvider
{
    [NotNull]
    Type[] GetDependedTypes();
}