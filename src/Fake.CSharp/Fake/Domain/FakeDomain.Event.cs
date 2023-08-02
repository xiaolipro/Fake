using System.Reflection;
using System.Runtime.Loader;

namespace Fake.CSharp.Fake.Domain;

public partial class FakeDomain
{
    private Assembly? DefaultResolving(AssemblyLoadContext arg1, AssemblyName arg2)
    {
        return Load(arg2);
    }

    private IntPtr DefaultResolvingUnmanagedDll(Assembly arg1, string arg2)
    {
        return LoadUnmanagedDll(arg2);
    }

    protected event Action<Assembly, string>? LoadAssemblyReferencesWithPath;
    protected event Action<Assembly, Stream>? LoadAssemblyReferencesWithStream;
}
