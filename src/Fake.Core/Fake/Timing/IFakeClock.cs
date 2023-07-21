namespace Fake.Timing;

public interface IFakeClock
{
    /// <summary>
    /// 当前时间
    /// </summary>
    DateTime Now { get; }
    
    /// <summary>
    /// 时间种类
    /// </summary>
    DateTimeKind Kind { get; }
    
    /// <summary>
    /// 标准化
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    DateTime Normalize(DateTime dateTime);

    /// <summary>
    /// 标准化成string
    /// </summary>
    /// <param name="datetime"></param>
    /// <returns></returns>
    string NormalizeAsString(DateTime datetime);
}