using Fake.Logging;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeServiceCollectionLoggingExtensions
{
    public static ILogger<T> GetLogger<T>(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IFakeLoggerFactory>().Create<T>();
    }
}