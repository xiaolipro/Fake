using System.Reflection.Emit;
using System.Collections.Concurrent;
using Fake.CSharp.Fake.Helpers;
using System.Diagnostics.Contracts;
using System.Reflection;
using Fake.CSharp.Fake.Domain.Extensions;

namespace Fake.CSharp.Fake.Domain;

public partial class FakeDomain
{
    private readonly ConcurrentDictionary<string, AssemblyName> _defaultAssemblyNameCache = new();
    private readonly HashSet<Assembly> _defaultAssembliesHashSet = new();
    private Func<AssemblyName, string?, bool> _excludeDefaultAssembliesFunc = (_, _) => false;
    public readonly FakeDomain DefaultDomain = default!;

    public void SetDefaultAssemblyFilter(Func<AssemblyName, string?, bool> filter)
    {
        _excludeDefaultAssembliesFunc = filter;
    }

    public void AddAssemblyToDefaultCache(Assembly assembly)
    {
        var assemblyName = assembly.GetName();
        if (!_excludeDefaultAssembliesFunc(assemblyName, assemblyName.Name))
        {
            _defaultAssemblyNameCache[assemblyName.GetUniqueName()] = assemblyName;

            lock (_defaultAssembliesHashSet)
            {
                _defaultAssembliesHashSet.Add(assembly);
            }
        }

#if DEBUG
        else
        {
            Debugger.WriteLine("忽略程序集", assemblyName.FullName);
        }
#endif
    }

    public void CheckAndIncrementAssemblies()
    {
        var assemblies = Default.Assemblies;
        var count = assemblies.Count();
        if (Interlocked.CompareExchange(ref _total, _preDefaultAssemblyCount, count) == count)
        {
            if (count != _preDefaultAssemblyCount)
            {
                _preDefaultAssemblyCount = count;
                HashSet<Assembly> checkAssemblyHashSet = new(Default.Assemblies);
                checkAssemblyHashSet.ExceptWith(_defaultAssembliesHashSet);

                foreach (var assembly in checkAssemblyHashSet)
                {
                    var assemblyName = assembly.GetName();

                    if (!_excludeDefaultAssembliesFunc(assemblyName, assemblyName.Name))
                    {
                        _defaultAssemblyNameCache[assemblyName.GetUniqueName()] = assemblyName;
                        _defaultAssembliesHashSet.Add(assembly);
                    }
#if DEBUG
                    else
                    {
                        Debugger.WriteLine("忽略程序集", assemblyName.FullName);
                    }
#endif
                }
            }
        }

    }

    private int _total = 0;

    private int _preDefaultAssemblyCount;

    public int DefaultAssemblyCacheCount => _preDefaultAssemblyCount;

    public void RefreshDefaultAssemblies(Func<AssemblyName, string?, bool>? filter)
    {
        if (filter is not null)
        {
            SetDefaultAssemblyFilter(filter);
        }

        CheckAndIncrementAssemblies();
    }
}
