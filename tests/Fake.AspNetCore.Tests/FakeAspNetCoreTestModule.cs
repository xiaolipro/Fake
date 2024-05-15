using Fake.AspNetCore.Testing;
using Fake.Autofac;
using Fake.Modularity;
using Fake.VirtualFileSystem;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.AspNetCore.Tests;

[DependsOn(
    typeof(FakeAutofacModule),
    typeof(FakeAspNetCoreTestingModule))]
public class FakeAspNetCoreTestModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<FakeVirtualFileSystemOptions>(options =>
        {
            options.FileProviders.Add<FakeAspNetCoreTestModule>("Fake.AspNetCore.Tests");
        });
    }

    public override void ConfigureApplication(ApplicationConfigureContext context)
    {
        var app = context.GetApplicationBuilder();
        var environment = context.GetEnvironmentOrNull();

        app.UseStaticFiles();
    }
}