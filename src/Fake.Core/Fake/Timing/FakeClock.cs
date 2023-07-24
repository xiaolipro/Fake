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
    /// ��ʼ��ʱ
    /// </summary>
    /// <returns>���ظö�ʱ����id</returns>
    /// <exception cref="ArgumentException">���ɳ���ͬid�������쳣</exception>
    public virtual Guid StartTimer()
    {
        var timerId = Guid.NewGuid();
        if (!_stopwatches.TryAdd(timerId, Stopwatch.StartNew())) throw new ArgumentException("�޷���Ӽ�ʱ�����Ѵ�����ͬid�ļ�ʱ��");
        return timerId;
    }

    /// <summary>
    /// ֹͣ��ʱ
    /// </summary>
    /// <param name="timerId">��ʱ��id</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">�Ҳ�����Ӧid�ļ�ʱ��ʱ�����쳣</exception>
    public virtual TimeSpan StopTimer(Guid timerId)
    {
        if (!_stopwatches.TryRemove(timerId, out var stopwatch)) throw new ArgumentException($"�Ҳ���idΪ:{timerId}�ļ�ʱ��");
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }
}