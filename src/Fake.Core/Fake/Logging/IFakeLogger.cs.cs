using Microsoft.Extensions.Logging;

namespace Fake.Logging;

public interface IFakeLogger<out T>: ILogger<T>
{
    public List<FakeLoggerEntry> Entries { get; }
}