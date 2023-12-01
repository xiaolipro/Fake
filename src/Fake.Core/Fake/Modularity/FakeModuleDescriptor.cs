using System.Collections.Immutable;
using System.Reflection;

namespace Fake.Modularity;

public class FakeModuleDescriptor : IModuleDescriptor
{
    public Type Type { get; }
    public Assembly Assembly { get; }
    public IFakeModule Instance { get; }

    private readonly List<IModuleDescriptor> _dependencies;
    public IReadOnlyList<IModuleDescriptor> Dependencies => _dependencies.ToImmutableList();

    public FakeModuleDescriptor(Type type, IFakeModule instance)
    {
        ThrowHelper.ThrowIfNull(type, nameof(type));
        ThrowHelper.ThrowIfNull(instance, nameof(instance));

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
        return $"[FakeModuleDescriptor {Type.FullName}]";
    }
}