using Microsoft.Extensions.DependencyInjection;

namespace Bang.Core.Modularity;

public interface IStartupModule
{
    void ConfigureServices(IServiceCollection services);
}