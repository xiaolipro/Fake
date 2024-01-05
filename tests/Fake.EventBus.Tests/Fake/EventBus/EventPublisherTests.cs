using Fake.DependencyInjection;
using Fake.EventBus.Events;
using Fake.Testing;
using Shouldly;

namespace Fake.EventBus;

public class EventPublisherTests : FakeApplicationTest<FakeEventBusTestModule>
{
    private readonly IEventPublisher _publisher;
    public static int Num { get; set; }

    public EventPublisherTests()
    {
        _publisher = GetRequiredService<IEventPublisher>();
    }

    [Fact]
    public void 发布事件()
    {
        _publisher.PublishAsync(new SimpleEvent(2));
        Num.ShouldBe(2);
    }
}

public class SimpleEvent(int i) : EventBase
{
    public override int Order { get; set; } = 1;
    public int Value { get; } = i;
}

public class SimpleEventHandler : IEventHandler<SimpleEvent>, ITransientDependency
{
    public int Order { get; set; }

    public Task HandleAsync(SimpleEvent @event, CancellationToken cancellationToken)
    {
        EventPublisherTests.Num += @event.Value;
        return Task.CompletedTask;
    }
}

public class Simple2EventHandler : IEventHandler<SimpleEvent>, ITransientDependency
{
    public int Order { get; set; } = -1;

    public Task HandleAsync(SimpleEvent @event, CancellationToken cancellationToken)
    {
        EventPublisherTests.Num *= @event.Value;
        return Task.CompletedTask;
    }
}