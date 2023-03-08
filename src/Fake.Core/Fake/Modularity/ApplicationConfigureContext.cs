using Fake.DependencyInjection;

namespace Fake.Modularity;

public class ApplicationConfigureContext : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    public ApplicationConfigureContext([NotNull] IServiceProvider serviceProvider)
    {
        ThrowHelper.ThrowIfNull(serviceProvider, nameof(serviceProvider));
        ServiceProvider = serviceProvider;
    }
}