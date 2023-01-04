namespace Bang.Modularity;

public interface IDependsOnProvider
{
    [NotNull]
    Type[] GetDependedTypes();
}