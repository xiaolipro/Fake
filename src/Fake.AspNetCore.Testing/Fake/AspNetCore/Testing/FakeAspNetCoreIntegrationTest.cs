using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fake.AspNetCore.Testing;

public abstract class FakeAspNetCoreIntegrationTest<TStartup> : FakeTestBase, IDisposable where TStartup : class
{
    protected TestServer Server { get; }
    
    protected HttpClient Client { get; }

    private readonly IHost _host;
    
    protected FakeAspNetCoreIntegrationTest()
    {
        var builder = CreateHostBuilder();

        _host = builder.Build();
        _host.Start();

        Server = _host.GetTestServer();
        Client = _host.GetTestClient();

        ServiceProvider = Server.Services;

        ServiceProvider.GetRequiredService<ITestServerAccessor>().Server = Server;
    }
    
    protected virtual IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .UseFakeTestServer<TStartup>()
            .UseAutofac()
            .ConfigureServices(ConfigureServices);
    }
    
    protected virtual void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {

    }
    
    public void Dispose()
    {
        _host?.Dispose();
    }
}