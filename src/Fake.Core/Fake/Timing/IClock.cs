namespace Fake.Timing;

public interface IClock
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
    /// 规格化
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    DateTime Normalize(DateTime dateTime);
}