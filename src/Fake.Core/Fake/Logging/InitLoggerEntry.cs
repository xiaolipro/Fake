using Microsoft.Extensions.Logging;

namespace Fake.Logging;

public class InitLoggerEntry
{
    public LogLevel LogLevel { get; set; }

    public EventId EventId { get; set; }

    public object? State { get; set; }

    public Exception? Exception { get; set; }

    public Func<object, Exception?, string> Formatter { get; set; } = null!;

    public string? Message => Formatter.Invoke(State ?? new(), Exception);
}