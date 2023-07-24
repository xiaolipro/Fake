using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace Fake.Timing;

public class FakeClock : IFakeClock
{
    private readonly FakeClockOptions _options;

    private readonly ConcurrentDictionary<Guid, Stopwatch> _stopwatches;

    public FakeClock(IOptions<FakeClockOptions> options)
    {
        _options = options.Value;
        _stopwatches = new ConcurrentDictionary<Guid, Stopwatch>();
    }
    
    public virtual DateTime Now  => _options.Kind == DateTimeKind.Utc ? DateTime.UtcNow : DateTime.Now;
    public virtual DateTimeKind Kind  => _options.Kind;
    
    public virtual DateTime Normalize(DateTime dateTime)
    {
        if (Kind == DateTimeKind.Unspecified || Kind == dateTime.Kind)
        {
            return dateTime;
        }

        return Kind switch
        {
            DateTimeKind.Local when dateTime.Kind == DateTimeKind.Utc => dateTime.ToLocalTime(),
            DateTimeKind.Utc when dateTime.Kind == DateTimeKind.Local => dateTime.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dateTime, Kind)
        };
    }

    public string NormalizeAsString(DateTime datetime)
    {
        return Normalize(datetime).ToString(_options.DateTimeFormat);
    }

    /// <summary>
    /// 开始计时
    /// </summary>
    /// <returns>返回该定时器的id</returns>
    /// <exception cref="ArgumentException">生成出相同id，会抛异常</exception>
    public virtual Guid StartTimer()
    {
        var timerId = Guid.NewGuid();
        if (!_stopwatches.TryAdd(timerId, Stopwatch.StartNew())) throw new ArgumentException("无法添加计时器，已存在相同id的计时器");
        return timerId;
    }

    /// <summary>
    /// 停止计时
    /// </summary>
    /// <param name="timerId">计时器id</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">找不到对应id的计时器时会抛异常</exception>
    public virtual TimeSpan StopTimer(Guid timerId)
    {
        if (!_stopwatches.TryRemove(timerId, out var stopwatch)) throw new ArgumentException($"找不到id为:{timerId}的计时器");
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }
}