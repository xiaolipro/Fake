namespace Fake.Modularity;

public interface IModuleContainer
{
    [NotNull]
    IReadOnlyList<IModuleDescriptor> Modules { get; }
}