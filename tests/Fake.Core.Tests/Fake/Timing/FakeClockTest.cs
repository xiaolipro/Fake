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
        var timerId = _fakeClock.StartTimer();
        Task.Delay(3000).GetAwaiter().GetResult();
        var time = _fakeClock.StopTimer(timerId);
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
}