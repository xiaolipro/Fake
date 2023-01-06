using Microsoft.Extensions.Logging;

namespace Bang.Logging;

public class DefaultBangLogger<T> : IBangLogger<T>
{
    public List<BangLoggerEntry> Entries { get; }
    
    public DefaultBangLogger()
    {
        Entries = new List<BangLoggerEntry>();
    }

    public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        Entries.Add(new BangLoggerEntry()
        {
            LogLevel = logLevel,
            EventId = eventId,
            State = state,
            Exception = exception,
            Formatter = (s, e) => formatter((TState)s, e),
        });
    }

    public virtual bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    public virtual IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        return NullDisposable.Instance;
    }
}