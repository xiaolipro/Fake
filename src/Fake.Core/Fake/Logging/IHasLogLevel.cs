using Microsoft.Extensions.Logging;

namespace Fake.Logging;

public interface IHasLogLevel
{
    /// <summary>
    /// 日志等级
    /// </summary>
    public LogLevel LogLevel { get; set; }
}