using System.Collections.Immutable;
using System.Reflection;

namespace Bang.Modularity;

public class BangModuleDescriptor:IBangModuleDescriptor
{
    public Type Type { get; }
    public Assembly Assembly { get; }
    public IBangModule Instance { get; }
    
    private readonly List<IBangModuleDescriptor> _dependencies;
    public IReadOnlyList<IBangModuleDescriptor> Dependencies => _dependencies.ToImmutableList();

    public BangModuleDescriptor([NotNull]Type type, [NotNull]IBangModule instance)
    {
        Check.NotNull(type, nameof(type));
        Check.NotNull(instance, nameof(instance));
        
        Type = type;
        Assembly = type.Assembly;
        Instance = instance;

        _dependencies = new List<IBangModuleDescriptor>();
    }
    
    public void AddDependency(IBangModuleDescriptor descriptor)
    {
        _dependencies.TryAdd(descriptor);
    }

    public override string ToString()
    {
        return $"[BangModuleDescriptor {Type.FullName}]";
    }
}