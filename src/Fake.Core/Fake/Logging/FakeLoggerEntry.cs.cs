using Microsoft.Extensions.Logging;

namespace Fake.Logging;

public class FakeLoggerEntry
{
    public LogLevel LogLevel { get; set; }

    public EventId EventId { get; set; }

    public object State { get; set; }

    public Exception Exception { get; set; }

    public Func<object, Exception, string> Formatter { get; set; }

    public string Message => Formatter(State, Exception);
}