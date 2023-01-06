using Bang.Modularity;
using Bang.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bang.Extensions;

internal static class InternalServiceCollectionExtensions
{
    internal static void AddBangCoreServices(this IServiceCollection services,IBangApplicationInfo applicationInfo, BangApplicationCreationOptions options)
    {
        services.TryAddSingleton<IModuleLoader>(new BangModuleLoader());

        var scanner = new BangAssemblyScanner(applicationInfo);
        services.TryAddSingleton<IAssemblyScanner>(scanner);
    }
}