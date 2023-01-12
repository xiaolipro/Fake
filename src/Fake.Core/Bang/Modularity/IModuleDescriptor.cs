using System.Reflection;

namespace Fake.Modularity;

public interface IModuleDescriptor
{
    Type Type { get; }

    Assembly Assembly { get; }

    IFakeModule Instance { get; }

    /// <summary>
    /// 依赖
    /// </summary>
    IReadOnlyList<IModuleDescriptor> Dependencies { get; }

    /// <summary>
    /// 添加依赖
    /// </summary>
    /// <param name="descriptor"></param>
    public void AddDependency(IModuleDescriptor descriptor);
}