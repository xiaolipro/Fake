namespace Bang.Logging;

public interface IBangLoggerFactory
{
    IBangLogger<T> Create<T>();
}