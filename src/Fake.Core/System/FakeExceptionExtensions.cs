using Fake.Logging;
using Microsoft.Extensions.Logging;

namespace System;

public static class FakeExceptionExtensions
{
    /// <summary>
    /// 如果<paramref name="exception"/>实现<see cref="IHasLogLevel"/>，则获取给定的日志等级，
    /// 否则，返回<paramref name="defaultLevel"/>
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="defaultLevel">默认日志等级</param>
    /// <returns></returns>
    public static LogLevel GetLogLevel(this Exception exception, LogLevel defaultLevel = LogLevel.Error)
    {
        return (exception as IHasLogLevel)?.LogLevel ?? defaultLevel;
    }
}