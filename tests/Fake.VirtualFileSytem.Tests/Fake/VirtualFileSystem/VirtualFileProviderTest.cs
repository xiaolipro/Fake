using System;
using System.Text;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Fake.VirtualFileSystem;

public class VirtualFileProviderTest:FakeIntegrationTest<FakeVirtualFileSystemTestModule>
{
    private readonly VirtualFileProvider _virtualFileProvider;

    public VirtualFileProviderTest()
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