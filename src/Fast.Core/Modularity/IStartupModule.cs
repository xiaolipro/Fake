using Microsoft.Extensions.DependencyInjection;

namespace Fast.Core.Modularity;

public interface IStartupModule
{
    void ConfigureServices(IServiceCollection services);

    public void Configure(IApplicationBuilder app);
}