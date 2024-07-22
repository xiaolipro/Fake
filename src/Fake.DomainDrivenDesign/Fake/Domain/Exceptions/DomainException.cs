using Fake.Logging;
using Microsoft.Extensions.Logging;

namespace Fake.Domain.Exceptions;

[Serializable]
public class DomainException(string? message = null, Exception? innerException = null)
    : FakeException(message, innerException), IHasLogLevel
{
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;
}