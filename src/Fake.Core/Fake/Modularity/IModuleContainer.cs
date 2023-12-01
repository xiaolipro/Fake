namespace Fake.Modularity;

public interface IModuleContainer
{
    IReadOnlyList<IModuleDescriptor> Modules { get; }
}