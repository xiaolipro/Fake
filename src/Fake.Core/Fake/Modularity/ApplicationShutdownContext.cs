using Fake.DependencyInjection;

namespace Fake.Modularity;

public class ApplicationShutdownContext: IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    public ApplicationShutdownContext([NotNull] IServiceProvider serviceProvider)
    {
        ThrowHelper.ThrowIfNull(serviceProvider, nameof(serviceProvider));
        ServiceProvider = serviceProvider;
    }
}