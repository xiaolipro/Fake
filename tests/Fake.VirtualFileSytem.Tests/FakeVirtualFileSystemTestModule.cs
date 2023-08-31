using Fake.Modularity;
using Fake.VirtualFileSystem;
using Microsoft.Extensions.DependencyInjection;

[DependsOn(typeof(FakeVirtualFileSystemModule))]
public class FakeVirtualFileSystemTestModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<FakeVirtualFileSystemOptions>(options =>
        {
            options.FileProviders.Add<FakeVirtualFileSystemTestModule>("/Assets");
        });
    }
}