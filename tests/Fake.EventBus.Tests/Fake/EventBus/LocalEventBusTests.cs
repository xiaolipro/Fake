using Fake.DependencyInjection;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.EventBus;

public class LocalEventBusTests : ApplicationTest<FakeEventBusTestModule>
{
    private readonly LocalEventBus _eventBus;

    public LocalEventBusTests()
    {
        SimpleLocalEventHandler.Init();
        _eventBus = ServiceProvider.GetRequiredService<LocalEventBus>();
    }

    [Fact]
    public async Task 发布本地事件无须订阅()
    {
        await _eventBus.PublishAsync(new SimpleEvent(1));
        await _eventBus.PublishAsync(new SimpleEvent(1));
        await _eventBus.PublishAsync(new SimpleEvent(1));

        Assert.Equal(3, SimpleLocalEventHandler.HandleCount);
    }


    [Fact]
    public async Task 事件处理者可以释放()
    {
        await _eventBus.PublishAsync(new SimpleEvent(1));
        await _eventBus.PublishAsync(new SimpleEvent(1));

        Assert.Equal(2, SimpleLocalEventHandler.DisposedCount);
    }

    [Fact]
    void dd()
    {
        ServiceProvider.GetRequiredService<LocalEventBus>();
    }
}

public class DD : IScopedDependency, IDisposable
{
    public void Dispose()
    {
        // TODO release managed resources here
    }
}