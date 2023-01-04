using Bang.Modularity;

namespace Bang.Core.DependencyInjection;

public interface IModuleLoader
{
    [NotNull]
    IBangModuleDescriptor[] LoadModules(
        [NotNull] IServiceCollection services,
        [NotNull] Type startupModuleType
    );
}