using System.Reflection;
using System.Runtime.Loader;
using Fake.CSharp.Fake.Helpers;

namespace Fake.CSharp.Fake.Domain;

public partial class FakeDomain
{
    private AssemblyDependencyResolver _assemblyDependencyResolver;
    private Func<AssemblyName, bool> _excludePluginReferencesFunc;

    public virtual Assembly LoadAssemblyFromFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException();
        }

#if DEBUG
        Debugger.WriteLine("加载路径", path);
#endif
        Assembly assembly;

        if (IsDefault())
        {
            assembly = Default.LoadFromAssemblyPath(path);
        }
        else
        {
            assembly = LoadFromAssemblyPath(path);
        }

        LoadAssemblyReferencesWithPath?.Invoke(assembly, path);

        return assembly;
    }

    public virtual Assembly LoadAssemblyFromStream(Stream stream, Stream? pdbStream)
    {
        using (stream)
        {
            Assembly assembly;

            if (IsDefault())
            {
                assembly = Default.LoadFromStream(stream, pdbStream);
            }
            else
            {
                assembly = LoadFromStream(stream, pdbStream);
            }

            stream.Seek(0, SeekOrigin.Begin);
            LoadAssemblyReferencesWithStream?.Invoke(assembly, stream);

            return assembly;
        }
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
#if DEBUG
        Debugger.WriteLine("解析程序集", $"AssemblyName: {assemblyName.Name}; FullName: {assemblyName.FullName}");
#endif

        var result = _excludePluginReferencesFunc(assemblyName);
        if (!result)
        {
            var assemblyPath = _assemblyDependencyResolver.ResolveAssemblyToPath(assemblyName);
            if (!string.IsNullOrWhiteSpace(assemblyPath))
            {
                return LoadAssemblyFromFile(assemblyPath);
            }
        }

        return null;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        var libraryPath = _assemblyDependencyResolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        if (!string.IsNullOrWhiteSpace(libraryPath))
        {
            return LoadUnmanagedDllFromPath(libraryPath);
        }

        return IntPtr.Zero;
    }
}
