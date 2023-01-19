using Xunit;
using Xunit.Abstractions;

namespace Fake.Auditing.Tests;

public class Class1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Class1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void dd()
    {
        _testOutputHelper.WriteLine(typeof(FakeAuditingModule).Namespace);
    }
}