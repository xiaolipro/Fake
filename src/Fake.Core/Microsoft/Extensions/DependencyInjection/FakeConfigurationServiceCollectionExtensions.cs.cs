using Fake;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeConfigurationServiceCollectionExtensions
{
    public static IServiceCollection ReplaceConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.Replace(ServiceDescriptor.Singleton(configuration));
    }

    public static IConfiguration GetConfiguration(this IServiceCollection services)
    {
        return services.GetConfigurationOrNull() ??
               throw new FakeException($"service collection中找不到{typeof(IConfiguration).AssemblyQualifiedName}的实现");
    }


    public static IConfiguration? GetConfigurationOrNull(this IServiceCollection services)
    {
        var hostBuilderContext = services.GetInstanceOrNull<HostBuilderContext>();
        if (hostBuilderContext?.Configuration != null)
        {
            return hostBuilderContext.Configuration as IConfigurationRoot;
        }

        return services.GetInstanceOrNull<IConfiguration>();
    }
}