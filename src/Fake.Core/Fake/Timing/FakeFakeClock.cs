using Fake.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fake.Timing;

public class FakeFakeClock : IFakeClock
{
    private readonly FakeClockOptions _options;

    public FakeFakeClock(IOptions<FakeClockOptions> options)
    {
        _options = options.Value;
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
}