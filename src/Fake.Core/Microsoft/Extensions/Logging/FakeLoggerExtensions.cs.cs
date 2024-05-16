namespace Microsoft.Extensions.Logging;

public static class FakeLoggerExtensions
{
    /// <summary>
    /// 异常日志
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    public static void LogException(this ILogger logger, Exception exception)
    {
        var logLevel = exception.GetLogLevel();

        logger.LogException(exception, logLevel);
    }

    /// <summary>
    /// 异常日志
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    /// <param name="logLevel"></param>
    public static void LogException(this ILogger logger, Exception exception, LogLevel logLevel)
    {
        logger.LogExceptionWithLevel(logLevel, exception);
    }

    /// <summary>
    /// 自定义级别异常日志
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="logLevel"></param>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    public static void LogExceptionWithLevel(this ILogger logger, LogLevel logLevel, Exception exception,
        string? message = null)
    {
        switch (logLevel)
        {
            case LogLevel.Critical:
                logger.LogCritical(exception, message ?? exception.Message);
                break;
            case LogLevel.Error:
                logger.LogError(exception, message ?? exception.Message);
                break;
            case LogLevel.Warning:
                logger.LogWarning(exception, message ?? exception.Message);
                break;
            case LogLevel.Information:
                logger.LogInformation(exception, message ?? exception.Message);
                break;
            case LogLevel.Debug:
                logger.LogDebug(exception, message ?? exception.Message);
                break;
            case LogLevel.Trace:
            case LogLevel.None:
            default: // LogLevel.Trace || LogLevel.None
                logger.LogDebug(exception, message ?? exception.Message);
                break;
        }
    }

    /// <summary>
    /// 自定义级别日志
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="logLevel"></param>
    /// <param name="message"></param>
    public static void LogWithLevel(this ILogger logger, LogLevel logLevel, string message)
    {
        switch (logLevel)
        {
            case LogLevel.Critical:
                logger.LogCritical(message);
                break;
            case LogLevel.Error:
                logger.LogError(message);
                break;
            case LogLevel.Warning:
                logger.LogWarning(message);
                break;
            case LogLevel.Information:
                logger.LogInformation(message);
                break;
            case LogLevel.Debug:
                logger.LogDebug(message);
                break;
            case LogLevel.Trace:
            case LogLevel.None:
            default: // LogLevel.Trace || LogLevel.None
                logger.LogDebug(message);
                break;
        }
    }
}