namespace Bang.Core.Logging;

public interface IBangLoggerFactory
{
    IBangLogger<T> Create<T>();
}