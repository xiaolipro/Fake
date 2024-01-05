using Fake.DependencyInjection;
using Fake.EventBus.Events;
using Fake.Testing;

namespace Fake.EventBus;

public class EventBusTests : FakeApplicationTest<FakeEventBusTestModule>
{
    private readonly IEventBus _eventBus;

    public EventBusTests()
    {
        _eventBus = GetRequiredService<IEventBus>();
    }

    [Fact]
    public async Task 发布本地事件()
    {
        _eventBus.Subscribe<SimpleEvent, SimpleLocalEventHandler>();

        await _eventBus.PublishAsync(new SimpleEvent(1));
        await _eventBus.PublishAsync(new SimpleEvent(2));
        await _eventBus.PublishAsync(new SimpleEvent(3));

        // 等待事件处理完成
        await Task.Delay(1000);

        Assert.Equal(3, SimpleLocalEventHandler.HandleCount);
        Assert.Equal(3, SimpleLocalEventHandler.DisposeCount);
    }
}

public class SimpleLocalEventHandler : IEventHandler<SimpleEvent>, IDisposable, ITransientDependency
{
    public int Order { get; set; }

    public static int HandleCount { get; set; }

    public static int DisposeCount { get; set; }

    public Task HandleAsync(SimpleEvent @event, CancellationToken cancellationToken)
    {
        HandleCount++;
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        DisposeCount++;
    }
}