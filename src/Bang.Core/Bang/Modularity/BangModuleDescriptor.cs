using System.Collections.Immutable;
using System.Reflection;

namespace Bang.Modularity;

public class BangModuleDescriptor:IModuleDescriptor
{
    public Type Type { get; }
    public Assembly Assembly { get; }
    public IBangModule Instance { get; }
    
    private readonly List<IModuleDescriptor> _dependencies;
    public IReadOnlyList<IModuleDescriptor> Dependencies => _dependencies.ToImmutableList();

    public BangModuleDescriptor([NotNull]Type type, [NotNull]IBangModule instance)
    {
        ThrowHelper.NotNull(type, nameof(type));
        ThrowHelper.NotNull(instance, nameof(instance));
        
        Type = type;
        Assembly = type.Assembly;
        Instance = instance;

        _dependencies = new List<IModuleDescriptor>();
    }
    
    public void AddDependency(IModuleDescriptor descriptor)
    {
        _dependencies.TryAdd(descriptor);
    }

    public override string ToString()
    {
        return $"[BangModuleDescriptor {Type.FullName}]";
    }
}