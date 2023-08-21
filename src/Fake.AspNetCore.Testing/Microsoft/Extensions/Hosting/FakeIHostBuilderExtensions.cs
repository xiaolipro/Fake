using System;
using Fake.AspNetCore.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

public static class FakeIHostBuilderExtensions
{
    public static IHostBuilder UseFakeTestServer<TStartup>(this IHostBuilder builder,
        Action<IWebHostBuilder> configure = null) where TStartup : class
    {
        return builder.ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<TStartup>();

            webBuilder.ConfigureServices(services =>
            {
                services.AddScoped<IHostLifetime, FakeNoopHostLifetime>();
                services.AddScoped<IServer, TestServer>();
            });
            
            configure(webBuilder);
        });
    }
}