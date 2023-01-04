namespace Bang.Modularity;

public class DependsOnAttribute : Attribute, IDependsOnProvider
{
    [NotNull]
    public Type[] DependedTypes { get; }
    
    public DependsOnAttribute(params Type[] dependedTypes)
    {
        DependedTypes = dependedTypes ?? Type.EmptyTypes;
    }
    
    public Type[] GetDependedTypes()
    {
        return DependedTypes;
    }
}