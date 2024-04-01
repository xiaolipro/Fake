using Fake;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeServiceProviderKeyedServiceExtensions
{
    public static object? GetKeyedService(this IServiceProvider provider, Type serviceType, object? serviceKey)
    {
        ThrowHelper.ThrowIfNull(provider, nameof(provider));

        if (provider is IKeyedServiceProvider keyedServiceProvider)
        {
            return keyedServiceProvider.GetKeyedService(serviceType, serviceKey);
        }

        throw new InvalidOperationException("This service provider doesn't support keyed services.");
    }
}