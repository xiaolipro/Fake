using Bang.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static class BangServiceCollectionLoggingExtensions
{
    public static ILogger<T> GetLogger<T>(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IBangLoggerFactory>().Create<T>();
    }
}