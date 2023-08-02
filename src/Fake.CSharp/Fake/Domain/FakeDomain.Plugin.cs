using System.Reflection;

namespace Fake.CSharp.Fake.Domain;

public partial class FakeDomain
{
    internal Assembly LoadPlugin(string path, Func<AssemblyName, bool>? excludeAssembliesFunc = null)
    {
        if (!File.Exists(path)) throw new FileNotFoundException();

        if (excludeAssembliesFunc is not null)
        {
            _excludePluginReferencesFunc = excludeAssembliesFunc;
        }

        CheckAndIncrementAssemblies();

        _assemblyDependencyResolver = new System.Runtime.Loader.AssemblyDependencyResolver(path);
        var assembly = LoadAssemblyFromFile(path);

        return assembly;
    }
}
