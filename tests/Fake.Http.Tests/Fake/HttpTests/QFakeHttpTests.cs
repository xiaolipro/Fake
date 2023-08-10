using Fake.Http.Fake.Http;
using Fake.Testing;
using Shouldly;
using Xunit.Abstractions;

namespace Fake.Http.Tests.Fake.HttpTests;

public class QFakeHttpTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public QFakeHttpTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task GetAsync()
    {
        // var res = await QFakeHttp.Url("https://www.baidu.com").GetAsync<string>();
        //
        // res.ShouldNotBeNull();
        //
        // _testOutputHelper.WriteLine(res);
    }
}

public class FakeHttpTests : FakeIntegrationTest<FakeHttpModule>
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IFakeHttp _fakeHttp;

    public FakeHttpTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _fakeHttp = GetRequiredService<IFakeHttp>();
    }

    [Fact]
    public async Task GetAsync()
    {
        var res = await _fakeHttp.Url("https://www.baidu.com").GetAsync<string>();
        
        res.ShouldNotBeNull();
        
        _testOutputHelper.WriteLine(res);
    }
}