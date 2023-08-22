using System;
using System.Net.Http;
using Fake.Modularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fake.AspNetCore.Testing;

public abstract class FakeAspNetCoreIntegrationTest<TStartupModule> : FakeTestBase, IDisposable
    where TStartupModule : class, IFakeModule
{
    protected TestServer Server { get; }

    protected HttpClient Client { get; }

    private readonly IHost _host;

    protected FakeAspNetCoreIntegrationTest()
    {
        var builder = CreateHostBuilder();

        var app = builder.Build();
        app.InitializeApplication();

        ConfigureApplication(app);
        _host = app;
        _host.Start();

        Server = _host.GetTestServer();
        Client = _host.GetTestClient();

        ServiceProvider = Server.Services;

        ServiceProvider.GetRequiredService<ITestServerAccessor>().Server = Server;
    }

    protected virtual void ConfigureApplication(WebApplication app)
    {
        
    }

    protected virtual WebApplicationBuilder CreateHostBuilder()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services
            .AddScoped<IHostLifetime, FakeNoopHostLifetime>()
            .AddScoped<IServer, TestServer>()
            .AddFakeApplication<TStartupModule>();
        builder.Host
            .UseAutofac()
            .ConfigureServices(ConfigureServices);

        return builder;
    }

    protected virtual void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
    }

    public void Dispose()
    {
        _host?.Dispose();
    }
}