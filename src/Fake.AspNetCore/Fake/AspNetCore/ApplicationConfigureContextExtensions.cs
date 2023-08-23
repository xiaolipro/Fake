using Fake.DependencyInjection;
using Fake.Modularity;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fake.AspNetCore;

public static class ApplicationConfigureContextExtensions
{
    public static IApplicationBuilder GetApplicationBuilder(this ApplicationConfigureContext context)
    {
        // 在加载FakeAspNetCoreModule模块ConfigureServices时
        var app = context.ServiceProvider.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value;
        ThrowHelper.ThrowIfNull(app, nameof(app),"请检查host是否是web host");
        return app;
    }
    
    [CanBeNull]
    public static IWebHostEnvironment GetEnvironmentOrNull(this ApplicationConfigureContext context)
    {
        return context.ServiceProvider.GetService<IWebHostEnvironment>();
    }
    
    public static IConfiguration GetConfiguration(this ApplicationConfigureContext context)
    {
        // 在构造FakeApplication时 services.AddFakeCoreServices(this, options);
        return context.ServiceProvider.GetRequiredService<IConfiguration>();
    }
    
    public static ILoggerFactory GetLoggerFactory(this ApplicationConfigureContext context)
    {
        // 在构造FakeApplication时 services.AddLogging();
        return context.ServiceProvider.GetRequiredService<ILoggerFactory>();
    }
}