namespace Fake.Timing;

public class FakeClockOptions
{
    /// <summary>
    /// 日期kind
    /// </summary>
    public DateTimeKind Kind { get; set; } = DateTimeKind.Unspecified;

    /// <summary>
    /// 日期显示格式
    /// </summary>
    public string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";
}