namespace Fake.ExceptionHandling;

public interface IExceptionSubscriber
{
    Task HandleAsync(ExceptionNotificationContext context);
}