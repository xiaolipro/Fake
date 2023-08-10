using Fake.Http.Fake.Http;
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