using Fake.Modularity;
using Fake.VirtualFileSystem;
using Microsoft.Extensions.DependencyInjection;

public class FakeVirtualFileSystemModule:FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<VirtualFileProvider>();
        context.Services.AddSingleton<DynamicFileProvider>();
    }
}