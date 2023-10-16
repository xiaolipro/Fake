
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Fake.AspNetCore.Endpoint;

public class EndpointTests:AspNetCoreTestBase
{
    [Fact]
    public async Task 测试服务()
    {
        var result = await GetResponseAsStringAsync(
            "/hi"
        );

        result.ShouldBe("hello world");
    }
}