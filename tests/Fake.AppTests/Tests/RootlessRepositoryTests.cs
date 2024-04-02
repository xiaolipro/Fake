using Domain.Aggregates.OrderAggregate;
using Domain.Queries;
using Fake.Modularity;
using Shouldly;
using Xunit;

namespace Tests;

public abstract class RootlessRepositoryTests<TStartupModule> : AppTestBase<TStartupModule>
    where TStartupModule : IFakeModule
{
    private readonly IOrderQueries _orderQueries;
    private readonly IOrderRepository _orderRepository;

    public RootlessRepositoryTests()
    {
        _orderQueries = ServiceProvider.GetRequiredService<IOrderQueries>();
        _orderRepository = ServiceProvider.GetRequiredService<IOrderRepository>();
    }

    [Fact]
    async Task GetOrderSummaryAsync()
    {
        var orders = await _orderQueries.GetOrderSummaryAsync(AppTestDataBuilder.OrderId);
        orders.Count.ShouldBe(1);
        orders.First().date.ShouldBeLessThanOrEqualTo(FakeClock.Now);
        orders[0].status.ShouldBe(OrderStatus.Submitted);
        orders[0].total.ShouldBe(20.4);
    }

    [Fact]
    async Task 无根仓储中写入会抛出异常()
    {
        var cnt = await _orderRepository.GetCountAsync();
        cnt.ShouldBe(1);

        var order = AppTestDataBuilder.BuildOrder();
        order.SetId(Guid.NewGuid());
        Should.Throw<InvalidOperationException>(async () =>
        {
            await _orderQueries.AddAsync(order);

            cnt = await _orderRepository.GetCountAsync();
            cnt.ShouldBe(2);
        });
    }
}