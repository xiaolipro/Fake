using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fake.ExceptionHandling;

public class ExceptionNotifier(IServiceScopeFactory serviceScopeFactory) : IExceptionNotifier
{
    public ILogger<ExceptionNotifier> Logger { get; set; } = NullLogger<ExceptionNotifier>.Instance;

    protected IServiceScopeFactory ServiceScopeFactory { get; } = serviceScopeFactory;

    public virtual async Task NotifyAsync(ExceptionNotificationContext context)
    {
        ThrowHelper.ThrowIfNull(context, nameof(context));

        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var exceptionSubscribers = scope.ServiceProvider
                .GetServices<IExceptionSubscriber>();

            foreach (var exceptionSubscriber in exceptionSubscribers)
            {
                try
                {
                    await exceptionSubscriber.HandleAsync(context);
                }
                catch (Exception e)
                {
                    Logger.LogWarning(
                        $"Exception subscriber of type {exceptionSubscriber.GetType().AssemblyQualifiedName} has thrown an exception!");
                    Logger.LogException(e, LogLevel.Warning);
                }
            }
        }
    }
}