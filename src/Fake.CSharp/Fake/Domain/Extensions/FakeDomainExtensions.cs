using System.Reflection;
using static System.Runtime.Loader.AssemblyLoadContext;

namespace Fake.CSharp.Fake.Domain.Extensions;

public static class FakeDomainExtensions
{
    public static ContextualReflectionScope CreateScope(this FakeDomain domain)
    {
        return domain.EnterContextualReflection();
    }

    public static Assembly LoadPluginWithHighDependency(this FakeDomain domain, string path, Func<AssemblyName, bool>? excludeAssembliesFunc = null)
    {
        if (!File.Exists(path)) throw new FileNotFoundException();

        return domain.LoadPlugin(path, excludeAssembliesFunc);
    }

    public static Assembly LoadPluginWithAllDependency(this FakeDomain domain, string path, Func<AssemblyName, bool>? excludeAssembliesFunc = null)
    {
        if (!File.Exists(path)) throw new FileNotFoundException();

        return domain.LoadPlugin(path, excludeAssembliesFunc);
    }
}
