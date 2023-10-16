using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fake.Timing;

public class FakeClockTest : FakeClockTestBase
{
    private readonly IFakeClock _fakeClock;

    public FakeClockTest()
    {
        _fakeClock = ServiceProvider.GetRequiredService<IFakeClock>();
    }

    [Fact]
    void 测试计时器()
    {
        var time = _fakeClock.MeasureExecutionTime(() =>
        {
            Thread.Sleep(3000);
        });
        
        Assert.True(time.Seconds is >= 3 and < 4);
    }
    
    [Fact]
    async Task 测试异步计时器()
    {
        var time = await _fakeClock.MeasureExecutionTimeAsync(async () =>
        {
            await Task.Delay(3000);
        });
        
        Assert.True(time.Seconds is >= 3 and < 4);
    }

    [Fact]
    async Task 并发测试计时器()
    {
        // 设置要并发执行的线程数
        int numThreads = 5;

        var tasks = new Task[numThreads];
        for (var i = 0; i < numThreads; i++)
        {
            tasks[i] = Task.Run(测试计时器);
        }

        await Task.WhenAll(tasks);
    }
    
    [Fact]
    async Task 并发测试异步计时器()
    {
        // 设置要并发执行的线程数
        int numThreads = 5;

        var tasks = new Task[numThreads];
        for (var i = 0; i < numThreads; i++)
        {
            tasks[i] = 测试异步计时器();
        }

        await Task.WhenAll(tasks);
    }
}