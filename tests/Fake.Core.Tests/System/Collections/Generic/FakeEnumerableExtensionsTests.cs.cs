using Fake.Helpers;

namespace Fake.Core.Tests.System.Collections.Generic;

public class FakeEnumerableExtensionsTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public FakeEnumerableExtensionsTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void WithIndex()
    {
        int cnt = 0;
        foreach (var (item, index) in Enumerable.Range(1, 10).WithIndex())
        {
            index.ShouldBe(cnt);
            cnt++;
            _testOutputHelper.WriteLine(index + "：" + item);
        }
    }

    [Fact]
    void JoinAsString()
    {
        Enumerable.Range(1, 3).JoinAsString(",").ShouldBe("1,2,3");
    }

    [Fact]
    public void SortByDependencies()
    {
        var dependencies = new Dictionary<char, char[]>
        {
            { 'A', new[] { 'B', 'G' } },
            { 'B', new[] { 'C', 'E' } },
            { 'C', new[] { 'D' } },
            { 'D', Array.Empty<char>() },
            { 'E', new[] { 'C', 'F' } },
            { 'F', new[] { 'C' } },
            { 'G', new[] { 'F' } }
        };

        for (int i = 0; i < 3; i++)
        {
            var list = RandomHelper.Shuffle(new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' });

            list = list.SortByDependencies(c => dependencies[c]);

            foreach (var dependency in dependencies)
            {
                foreach (var dependedValue in dependency.Value)
                {
                    // 先构建依赖项
                    list.IndexOf(dependency.Key).ShouldBeGreaterThan(list.IndexOf(dependedValue));
                }
            }
        }
    }

    [Fact]
    public void SortByDependencies有环会抛出异常()
    {
        var dependencies = new Dictionary<char, char[]>
        {
            { 'A', new[] { 'B', 'G' } },
            { 'B', new[] { 'C', 'A' } },
            { 'C', new[] { 'D' } },
            { 'D', Array.Empty<char>() },
            { 'E', new[] { 'C', 'F' } },
            { 'F', new[] { 'C' } },
            { 'G', new[] { 'F' } }
        };
        var list = RandomHelper.Shuffle(new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' });

        var res = Should.Throw<ArgumentException>(() => { list = list.SortByDependencies(c => dependencies[c]); });

        _testOutputHelper.WriteLine(res.Message);
    }
}