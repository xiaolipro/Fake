using System.Collections.Immutable;
using System.Reflection;
using Bang.Modularity;

namespace Bang.Reflection;

/// <summary>
/// 程序集扫描器
/// </summary>
public interface IAssemblyScanner
{
    IReadOnlyList<Assembly> Scan();
}

/// <summary>
/// 扫描所有Bang模块相关的程序集
/// </summary>
public class BangAssemblyScanner : IAssemblyScanner
{
    private readonly IModuleContainer _moduleContainer;
    private readonly Lazy<IReadOnlyList<Assembly>> _assemblies;

    public BangAssemblyScanner(IModuleContainer moduleContainer)
    {
        _moduleContainer = moduleContainer;

        _assemblies = new Lazy<IReadOnlyList<Assembly>>(FindAssemblies, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    public IReadOnlyList<Assembly> Scan()
    {
        return _assemblies.Value;
    }

    private IReadOnlyList<Assembly> FindAssemblies()
    {
        var assemblies = new List<Assembly>();

        foreach (var module in _moduleContainer.Modules)
        {
            assemblies.Add(module.Type.Assembly);
        }

        return assemblies.Distinct().ToImmutableList();
    }
}