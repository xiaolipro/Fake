using Fake.IdGenerators.Snowflake;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Core.Tests.IdGenerators;

public class SnowflakeIdGeneratorTests : ApplicationTestWithTools<FakeCoreTestModule>
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly SnowflakeIdGenerator _longIdGenerator;

    public SnowflakeIdGeneratorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _longIdGenerator = ServiceProvider.GetRequiredService<SnowflakeIdGenerator>();
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
    [InlineData(10000)]
    void 雪花Id具有单调递增性(int iteratorNum)
    {
        var ids = new List<long>(iteratorNum);

        for (int i = 0; i < iteratorNum; i++)
        {
            var id = _longIdGenerator.Generate();
            ids.Add(id);
        }

        var sorted = true;
        for (int i = 1; i < ids.Count; i++)
        {
            if (ids[i] <= ids[i - 1])
            {
                sorted = false;
                _testOutputHelper.WriteLine($"ids[{i}] {ids[i]} <= ids[{i - 1}] {ids[i - 1]}");
            }
        }

        sorted.ShouldBe(true);
    }

    [Theory]
    [InlineData(6, 10000)]
    void 雪花Id在并发下无法保证全局单调性(int threadNum, int iteratorNum)
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
        for (int i = 1; i < ids.Count; i++)
        {
            if (ids[i] <= ids[i - 1])
            {
                sorted = false;
                _testOutputHelper.WriteLine($"ids[{i}] {ids[i]} <= ids[{i - 1}] {ids[i - 1]}");
            }
        }

        sorted.ShouldBe(false);
    }
}