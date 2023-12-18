using System;
using Fake.AspNetCore.Testing;
using Fake.Modularity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

public static class FakeIHostBuilderExtensions
{
    public static IHostBuilder UseFakeTestServer<TStartupModule>(this IHostBuilder builder,
        Action<IWebHostBuilder>? configure = null) where TStartupModule : IFakeModule
    {
        return builder.ConfigureWebHostDefaults(webBuilder =>
        {
            // webBuilder.UseTestServer();
            webBuilder.ConfigureServices(services =>
            {
                // 替换主机生命周期为无操作（No-op）主机生命周期
                // 在集成测试中，不希望应用程序真正启动和运行，只需要进行一些初始化操作
                services.AddScoped<IHostLifetime, FakeNoopHostLifetime>();
                services.AddScoped<IServer, TestServer>();

                services.AddFakeApplication<TStartupModule>();
            });

            configure?.Invoke(webBuilder);
        });
    }
}