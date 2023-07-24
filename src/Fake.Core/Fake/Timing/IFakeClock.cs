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

    /// <summary>
    /// 开始计时
    /// </summary>
    /// <returns>返回该定时器的id</returns>
    Guid StartTimer();

    /// <summary>
    /// 停止计时
    /// </summary>
    /// <param name="timerId">计时器id</param>
    /// <returns>返回计时时间</returns>
    TimeSpan StopTimer(Guid timerId);
}