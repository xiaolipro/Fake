using Microsoft.Extensions.Logging;

namespace Fake.Logging;

public class FakeInitLogger<T> : IInitLogger<T>
{
    public List<InitLoggerEntry> Entries { get; }
    
    public FakeInitLogger()
    {
        Entries = new List<InitLoggerEntry>();
    }

    public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        Entries.Add(new InitLoggerEntry()
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