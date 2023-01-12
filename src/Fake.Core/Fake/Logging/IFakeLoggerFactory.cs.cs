namespace Fake.Logging;

public interface IFakeLoggerFactory
{
    IFakeLogger<T> Create<T>();
}