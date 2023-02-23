namespace Fake.ExceptionHandling;

/// <summary>
/// 空的<see cref="IExceptionNotifier"/>实现
/// </summary>
public class NullExceptionNotifier :IExceptionNotifier
{
    public static NullExceptionNotifier Instance { get; } = new NullExceptionNotifier();

    private NullExceptionNotifier()
    {

    }
    
    public Task NotifyAsync(ExceptionNotificationContext context)
    {
        return Task.CompletedTask;
    }
}