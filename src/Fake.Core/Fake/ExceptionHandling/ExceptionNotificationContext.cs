using Microsoft.Extensions.Logging;

namespace Fake.ExceptionHandling;

public class ExceptionNotificationContext
{
    [NotNull]
    public Exception Exception { get; }
    
    public LogLevel LogLevel { get; }
    
    public bool ExceptionHandled { get; }
    
    public ExceptionNotificationContext(
        [NotNull] Exception exception,
        LogLevel? logLevel = null,
        bool handled = true)
    {
        Exception = ThrowHelper.ThrowIfNull(exception, nameof(exception));
        LogLevel = logLevel ?? exception.GetLogLevel();
        ExceptionHandled = handled;
    }
}