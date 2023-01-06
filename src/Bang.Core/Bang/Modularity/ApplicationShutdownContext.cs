using Bang.Core.DependencyInjection;

namespace Bang.Modularity;

public class ApplicationShutdownContext: IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    public ApplicationShutdownContext([NotNull] IServiceProvider serviceProvider)
    {
        ThrowHelper.NotNull(serviceProvider, nameof(serviceProvider));
        ServiceProvider = serviceProvider;
    }
}