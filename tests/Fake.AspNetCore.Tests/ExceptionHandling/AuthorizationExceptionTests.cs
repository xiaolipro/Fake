using System.Net;
using Fake.AspNetCore.ExceptionHandling;
using Microsoft.AspNetCore.Builder;
using Xunit;
using Xunit.Abstractions;

namespace Fake.AspNetCore.Tests.ExceptionHandling;

public class AuthorizationExceptionTests : AspNetCoreTestBase
{
    private readonly ITestOutputHelper _testOutputHelper;
    private SimpleService _simpleService;

    public AuthorizationExceptionTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _simpleService = GetRequiredService<SimpleService>();
    }

    protected override void ConfigureApplication(WebApplication app)
    {
        base.ConfigureApplication(app);
        app.UseFakeExceptionHandling();

        app.MapGet("auth-exception", () => _simpleService.AuthorizationException());
    }

    [Fact]
    public async Task 测试服务()
    {
        var res = await GetResponseAsStringAsync("auth-exception", HttpStatusCode.InternalServerError);
        _testOutputHelper.WriteLine(res.ToString());
    }
}