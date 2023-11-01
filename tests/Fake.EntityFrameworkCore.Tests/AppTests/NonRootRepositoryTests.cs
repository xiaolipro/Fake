using Domain.Aggregates.OrderAggregate;
using Domain.Queries;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace AppTests;

public class NonRootRepositoryTests : AppTestBase<FakeEntityFrameworkCoreTestModule>
{
    private readonly IOrderQueries _orderQueries;
    private readonly IOrderRepository _orderRepository;

    public NonRootRepositoryTests()
    {
        _orderQueries = GetRequiredService<IOrderQueries>();
        _orderRepository = GetRequiredService<IOrderRepository>();
    }

    protected override void BeforeAddFakeApplication(IServiceCollection services)
    {
        services.AddTransient<IOrderQueries, OrderQueries>();
    }

    [Fact]
    async Task GetOrderSummaryAsync()
    {
        var orders = await _orderQueries.GetOrderSummaryAsync(AppTestDataBuilder.OrderId);
        orders.Count.ShouldBe(1);
        orders.First().date.ShouldBeLessThanOrEqualTo(FakeClock.Now);
        orders[0].status.ShouldBe(OrderStatus.Submitted.Name);
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


    [Fact]
    async Task 无根仓储中用SQL写入不会抛出异常()
    {
        var cnt = await _orderRepository.GetCountAsync();
        cnt.ShouldBe(1);

        var order = AppTestDataBuilder.BuildOrder();
        order.SetId(Guid.NewGuid());
        await _orderQueries.AddBySqlAsync(order);
    }
}