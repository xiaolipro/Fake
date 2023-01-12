namespace Fake.Modularity;

public interface IModuleLoader
{
    [NotNull]
    IModuleDescriptor[] LoadModules(
        [NotNull] IServiceCollection services,
        [NotNull] Type startupModuleType
    );
}