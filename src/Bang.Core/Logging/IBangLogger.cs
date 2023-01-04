using Microsoft.Extensions.Logging;

namespace Bang.Core.Logging;

public interface IBangLogger<out T>: ILogger<T>
{
    public List<BangLoggerEntry> Entries { get; }
}