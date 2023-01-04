using System.Reflection;

namespace Bang.Modularity;

public interface IBangModuleDescriptor
{
    Type Type { get; }

    Assembly Assembly { get; }

    IBangModule Instance { get; }

    /// <summary>
    /// 依赖
    /// </summary>
    IReadOnlyList<IBangModuleDescriptor> Dependencies { get; }

    /// <summary>
    /// 添加依赖
    /// </summary>
    /// <param name="descriptor"></param>
    public void AddDependency(IBangModuleDescriptor descriptor);
}