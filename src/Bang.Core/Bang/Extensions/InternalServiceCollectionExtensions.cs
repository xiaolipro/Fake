using Bang.Modularity;
using Bang.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bang.Extensions;

internal static class InternalServiceCollectionExtensions
{
    internal static void AddBangCoreServices(this IServiceCollection services,IBangApplicationInfo applicationInfo, BangApplicationCreationOptions options)
    {
        services.TryAddSingleton<IModuleLoader>(new BangModuleLoader());

        var assemblyScanner = new BangAssemblyScanner(applicationInfo);
        services.TryAddSingleton<IAssemblyScanner>(assemblyScanner);
        var typeScanner = new BangAssemblyTypeScanner(assemblyScanner);
        services.TryAddSingleton<ITypeScanner>(typeScanner);

        if (services.<IConfiguration>())
        {
            
        }
    }
}