using Fake.AspNetCore.Testing;
using Microsoft.AspNetCore.Builder;

namespace Fake.AspNetCore;

public class AspNetCoreTestBase:FakeAspNetCoreIntegrationTestWithTools<FakeAspNetCoreTestModule>
{
    protected override void ConfigureApplication(WebApplication app)
    {
        app.MapGet("/hi", () => "hello world");
    }
}