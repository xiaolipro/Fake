using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.VirtualFileSystem;
public class FakeVirtualFileSystemModule:FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<VirtualFileProvider>();
        context.Services.AddSingleton<DynamicFileProvider>();
    }
}