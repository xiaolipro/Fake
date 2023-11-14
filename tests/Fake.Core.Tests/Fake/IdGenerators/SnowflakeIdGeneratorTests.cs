using Fake.IdGenerators.Snowflake;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.IdGenerators;

public class SnowflakeIdGeneratorTests : FakeIntegrationTestWithTools<FakeCoreTestModule>
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly SnowflakeIdGenerator _longIdGenerator;

    public SnowflakeIdGeneratorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _longIdGenerator = ServiceProvider.GetService<SnowflakeIdGenerator>();
    }

    [Fact]
    void 生成雪花Id()
    {
        _longIdGenerator.Generate().ShouldNotBe(0);
    }

    [Theory]
    [InlineData(6, 10000)]
    void 雪花Id具有唯一性(int threadNum, int iteratorNum)
    {
        var ids = new List<long>(threadNum * iteratorNum);

        Parallel.ForEach(Enumerable.Range(1, threadNum), t =>
        {
            for (int i = 0; i < iteratorNum; i++)
            {
                var id = _longIdGenerator.Generate();
                ids.Add(id);
            }
        });

        var oldLen = ids.Count;

        ids.Distinct().Count().ShouldBe(oldLen);
    }


    [Theory]
    [InlineData(6, 10000)]
    void 雪花Id具有单调递增性(int threadNum, int iteratorNum)
    {
        var ids = new List<long>(threadNum * iteratorNum);

        Parallel.ForEach(Enumerable.Range(1, threadNum), t =>
        {
            for (int i = 0; i < iteratorNum; i++)
            {
                var id = _longIdGenerator.Generate();
                ids.Add(id);
            }
        });

        var sorted = true;
        for (int i = 1; i <= ids.Count; i++)
        {
            if (ids[i] <= ids[i - 1])
            {
                sorted = false;
                _testOutputHelper.WriteLine($"ids[{i}] {ids[i]} <= ids[{i - 1}] {ids[i - 1]}");
            }
        }

        sorted.ShouldBe(true);
    }
}