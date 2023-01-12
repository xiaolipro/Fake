namespace Fake.Logging;

public interface IInitLoggerFactory
{
    IInitLogger<T> Create<T>();
}