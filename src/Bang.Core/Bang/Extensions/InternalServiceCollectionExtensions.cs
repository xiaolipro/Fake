using Bang.Logging;
using Bang.Modularity;
using Bang.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bang.Extensions;

internal static class InternalServiceCollectionExtensions
{
    internal static void AddBangCoreServices(this IServiceCollection services,IBangApplicationInfo applicationInfo, BangApplicationCreationOptions options)
    {
        // 配置文件
        if (!services.IsAdded<IConfiguration>())
        {
            var configuration = BangConfigurationHelper.Build();
            services.ReplaceConfiguration(configuration);
        }
        
        var assemblyScanner = new BangAssemblyScanner(applicationInfo);
        var typeScanner = new BangAssemblyTypeScanner(assemblyScanner);
        
        services.TryAddSingleton<IModuleLoader>(new BangModuleLoader());
        services.TryAddSingleton<IAssemblyScanner>(assemblyScanner);
        services.TryAddSingleton<ITypeScanner>(typeScanner);
        services.TryAddSingleton<IBangLoggerFactory>(new DefaultBangLoggerFactory());
    }
}