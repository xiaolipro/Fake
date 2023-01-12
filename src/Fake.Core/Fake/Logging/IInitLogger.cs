using Microsoft.Extensions.Logging;

namespace Fake.Logging;

public interface IInitLogger<out T>: ILogger<T>
{
    public List<InitLoggerEntry> Entries { get; }
}