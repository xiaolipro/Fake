namespace Fake.Modularity;

public interface IModuleLoader
{
    IModuleDescriptor[] LoadModules(
        IServiceCollection services,
        Type startupModuleType
    );
}