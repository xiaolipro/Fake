using System.Reflection;
using Fake.DependencyInjection;
using Fake.Logging;
using Fake.Modularity;
using Fake.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fake.Extensions;

internal static class InternalServiceCollectionExtensions
{
    internal static void AddFakeCoreServices(this IServiceCollection services,IFakeApplicationInfo applicationInfo, FakeApplicationCreationOptions options)
    {
        // 配置文件
        if (!services.IsAdded<IConfiguration>())
        {
            var configuration = FakeConfigurationHelper.Build();
            services.ReplaceConfiguration(configuration);
        }
        
        var assemblyScanner = new FakeAssemblyScanner(applicationInfo);
        var typeScanner = new FakeAssemblyTypeScanner(assemblyScanner);
        
        services.TryAddSingleton<IModuleLoader>(new FakeModuleLoader());
        services.TryAddSingleton<IAssemblyScanner>(assemblyScanner);
        services.TryAddSingleton<ITypeScanner>(typeScanner);
        
        services.TryAddSingleton<IFakeLoggerFactory>(new DefaultFakeLoggerFactory());
    }

    internal static void ServiceRegister(this IServiceCollection services, Assembly assembly)
    {
        
    }
}