using System.Collections.Concurrent;
using Fast.Core.Helpers;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Fast.Core.Tests.Helpers;

public class RandomHelperTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public RandomHelperTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void Next()
    {
        var rand = new Random();
        var bag = new ConcurrentBag<int>();
        var bag2 = new ConcurrentBag<int>();
        int cnt = 0;
        Task.Run(() =>
        {
            for (int i = 0; i < 100000; i++)
            {
                bag.Add(rand.Next());
                bag2.Add(RandomHelper.Next());
                // ReSharper disable once AccessToModifiedClosure
                Interlocked.Increment(ref cnt);
            }
        });
        
        for (int i = 0; i < 100000; i++)
        {
            bag.Add(rand.Next());
            bag2.Add(RandomHelper.Next());
            Interlocked.Increment(ref cnt);
        }

        int a = bag.ToHashSet().Count, b = bag2.ToHashSet().Count;
        _testOutputHelper.WriteLine("total: " + cnt + ", after set for bag: " + a+ ", after set for bag2: " + b);
        a.ShouldBeLessThanOrEqualTo(b);
    }
}