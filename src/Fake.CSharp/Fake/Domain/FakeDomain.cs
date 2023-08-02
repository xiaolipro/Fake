using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using Fake.CSharp.Fake.Exceptions;

namespace Fake.CSharp.Fake.Domain;

public partial class FakeDomain : AssemblyLoadContext, IDisposable
{
    private const string DEFAULT_DOMAIN = "DEFAULT";

    public FakeDomain() : base(DEFAULT_DOMAIN)
    {
        Default.Resolving += DefaultResolving;
        Default.ResolvingUnmanagedDll += DefaultResolvingUnmanagedDll;

        _excludePluginReferencesFunc = item => false;
        _assemblyDependencyResolver = new AssemblyDependencyResolver(AppDomain.CurrentDomain.BaseDirectory);
        Unsafe.AsRef(DefaultDomain) = this;
    }

    public FakeDomain(string domain) : base(domain, true)
    {
        if (IsDefault())
        {
            throw new DefaultDomainException("不能重复创建默认域");
        }

        _excludePluginReferencesFunc = item => false;
        _assemblyDependencyResolver = new AssemblyDependencyResolver(AppDomain.CurrentDomain.BaseDirectory);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }


    private bool IsDefault()
    {
        return Name?.Equals(DEFAULT_DOMAIN, StringComparison.OrdinalIgnoreCase) ?? false;
    }
}
