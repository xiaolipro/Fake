using System.Reflection;
using Fake.Logging;
using Fake.Modularity;
using Fake.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Fake.Extensions;

internal static class InternalServiceCollectionExtensions
{
    internal static void AddFakeCoreServices(this IServiceCollection services,IFakeApplication application, FakeApplicationCreationOptions creationOptions)
    {
        IConfiguration configuration = null;
        // 为非Web应用加载配置文件
        if (!services.IsAdded<IConfiguration>())
        {
            configuration = FakeConfigurationHelper.BuildConfiguration(creationOptions.Configuration);
            services.ReplaceConfiguration(configuration);
        }

        // 配置日志
        services.AddLogging(logging =>
        {
            logging.AddConfiguration(configuration!.GetSection("Logging"));
            logging.AddConsole();
        });

        var assemblyScanner = new FakeAssemblyScanner(application);
        var typeScanner = new FakeAssemblyTypeScanner(assemblyScanner);
        
        services.TryAddSingleton<IModuleLoader>(new FakeModuleLoader());
        services.TryAddSingleton<IAssemblyScanner>(assemblyScanner);
        services.TryAddSingleton<ITypeScanner>(typeScanner);
        
        services.TryAddSingleton<IInitLoggerFactory>(new DefaultInitLoggerFactory());
    }

    internal static void ServiceRegister(this IServiceCollection services, Assembly assembly)
    {
        
    }
}