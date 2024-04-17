using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.VirtualFileSystem.Tests;

[DependsOn(typeof(FakeVirtualFileSystemModule))]
public class FakeVirtualFileSystemTestModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<FakeVirtualFileSystemOptions>(options =>
        {
            options.FileProviders.Add<FakeVirtualFileSystemTestModule>("Fake.VirtualFileSystem.Tests.Assets");
        });
    }
}