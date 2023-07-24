using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace Fake.Timing;

public sealed class FakeClock : IFakeClock
{
    private readonly FakeClockOptions _options;

    private readonly AsyncLocal<Stopwatch> _stopwatches;

    public FakeClock(IOptions<FakeClockOptions> options)
    {
        _options = options.Value;
        _stopwatches = new AsyncLocal<Stopwatch>();
    }
    
    public DateTime Now  => _options.Kind == DateTimeKind.Utc ? DateTime.UtcNow : DateTime.Now;
    public DateTimeKind Kind  => _options.Kind;
    
    public DateTime Normalize(DateTime dateTime)
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
    public void StartTimer()
    {
        if (_stopwatches.Value == null)
        {
            _stopwatches.Value = Stopwatch.StartNew();
            return;
        }
        
        _stopwatches.Value.Restart();
    }

    /// <summary>
    /// 停止计时
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException">找不到计时器时会抛异常</exception>
    public TimeSpan StopTimer()
    {
        if (_stopwatches.Value == null) throw new ArgumentException("找不到计时器, 是否启用了计时器");

        var stopwatch = _stopwatches.Value;
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }
}