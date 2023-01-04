using Bang.Core.DependencyInjection;

namespace Bang.Modularity;

public class ApplicationInitializationContext : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    public ApplicationInitializationContext([NotNull] IServiceProvider serviceProvider)
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));
        ServiceProvider = serviceProvider;
    }
}