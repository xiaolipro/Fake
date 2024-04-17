using Fake.DependencyInjection;
using Fake.EventBus.Events;

namespace Fake.EventBus.Tests.Events;

public class SimpleLocalEventHandler : IEventHandler<SimpleEvent>, ITransientDependency, IDisposable
{
    public static int HandleCount { get; set; }
    public static int DisposedCount { get; set; }

    public static void Init()
    {
        HandleCount = 0;
        DisposedCount = 0;
    }

    public Task HandleAsync(SimpleEvent @event, CancellationToken cancellationToken)
    {
        HandleCount++;
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        DisposedCount++;
    }
}