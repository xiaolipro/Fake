
using System;
using System.Text;
using Fake.Testing;
using Fake.VirtualFileSystem;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

public class VirtualFileProviderTests:FakeIntegrationTest<FakeVirtualFileSystemTestModule>
{
    private readonly VirtualFileProvider _virtualFileProvider;

    public VirtualFileProviderTests()
    {
        _virtualFileProvider = ServiceProvider.GetRequiredService<VirtualFileProvider>();
    }

    [Fact]
    void 嵌入文件()
    {
        //action
        var resource = _virtualFileProvider.GetFileInfo("js/jquery.js");
        
        //assert
        resource.ShouldNotBeNull();
        resource.Exists.ShouldBeTrue();
        
        using (var stream = resource.CreateReadStream())
        {
            Encoding.UTF8.GetString(stream.ReadAsBytes()).ShouldBe("//jquery.js content");
        }
    }
}