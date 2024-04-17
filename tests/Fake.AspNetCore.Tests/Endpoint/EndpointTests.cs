using Microsoft.AspNetCore.Builder;
using Shouldly;
using Xunit;

namespace Fake.AspNetCore.Tests.Endpoint;

public class EndpointTests : AspNetCoreTestBase
{
    protected override void ConfigureApplication(WebApplication app)
    {
        base.ConfigureApplication(app);
        app.MapGet("/hi", () => "hello world");
    }

    [Fact]
    public async Task 测试服务()
    {
        var result = await GetResponseAsStringAsync(
            "/hi"
        );

        result.ShouldBe("hello world");
    }
}