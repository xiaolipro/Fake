namespace Fake.Timing;

public class FakeClockOptions
{
    /// <summary>
    /// 默认未知的: <see cref="DateTimeKind.Unspecified"/>
    /// </summary>
    public DateTimeKind Kind { get; set; }

    public FakeClockOptions()
    {
        Kind = DateTimeKind.Unspecified;
    }
}