using System.Text;
using Fake.Modularity;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Shouldly;
using Xunit;

namespace Fake.VirtualFileSystem.Tests;

public class DynamicFileProviderBaseTests : ApplicationTest<DynamicFileTestModule>
{
    private readonly IDynamicFileProvider _dynamicFileProvider;

    public DynamicFileProviderBaseTests()
    {
        _dynamicFileProvider = ServiceProvider.GetRequiredService<IDynamicFileProvider>();
    }

    [Fact]
    public void 创建动态文件()
    {
        const string fileContent = "Hello World";

        _dynamicFileProvider.AddOrUpdate(
            new InMemoryFileInfo(
                "/my-files/test.txt",
                "Hello World"
            )
        );

        var fileInfo = _dynamicFileProvider.GetFileInfo("/my-files/test.txt");
        fileInfo.ShouldNotBeNull();
        fileInfo.ReadAsString(Encoding.UTF8).ShouldBe(fileContent);
    }

    [Fact]
    public void 动态文件支持监听变更()
    {
        var path = "/my-files/test.txt";

        //Create a dynamic file
        _dynamicFileProvider.AddOrUpdate(
            new InMemoryFileInfo(path, "Hello World")
        );

        //Register to change on that file
        var fileCallbackCalled = false;
        ChangeToken.OnChange(
            () => _dynamicFileProvider.Watch(path),
            () => { fileCallbackCalled = true; });

        //Updating the file should trigger the callback
        _dynamicFileProvider.AddOrUpdate(
            new InMemoryFileInfo(path, "Hello World A")
        );
        fileCallbackCalled.ShouldBeTrue();

        //Updating the file should trigger the callback (2nd test)
        fileCallbackCalled = false;
        _dynamicFileProvider.AddOrUpdate(
            new InMemoryFileInfo(path, "Hello World B")
        );
        fileCallbackCalled.ShouldBeTrue();
    }
}

[DependsOn(typeof(FakeVirtualFileSystemModule))]
public class DynamicFileTestModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}