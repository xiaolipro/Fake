namespace Fake.ExceptionHandling;

public class ExceptionNotificationContext(
    Exception exception,
    IServiceProvider serviceProvider)
{
    public Exception Exception { get; } = ThrowHelper.ThrowIfNull(exception, nameof(exception));

    public IServiceProvider ServiceProvider { get; } = serviceProvider;
}