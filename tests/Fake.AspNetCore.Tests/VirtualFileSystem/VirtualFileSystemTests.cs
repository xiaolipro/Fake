using Shouldly;
using Xunit;

namespace Fake.AspNetCore.Tests.VirtualFileSystem;

public class VirtualFileSystemTests : AspNetCoreTestBase
{
    [Fact]
    public async Task 请求虚拟文件()
    {
        var result = await GetResponseAsStringAsync(
            "asset/hi.txt"
        );

        result.ShouldBe("hello world");
    }
}