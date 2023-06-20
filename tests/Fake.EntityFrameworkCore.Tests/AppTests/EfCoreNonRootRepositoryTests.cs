using Domain.Aggregates.OrderAggregate;
using Domain.Aggregates.QueriesRepositories;
using Fake;
using Fake.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Shouldly;
using Xunit;

namespace AppTests;

public class EfCoreNonRootRepositoryTests: AppTestBase<FakeEntityFrameworkCoreTestModule>
{
    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly IOrderRepository _orderRepository;
    public EfCoreNonRootRepositoryTests()
    {
        _orderQueryRepository = GetRequiredService<IOrderQueryRepository>();
        _orderRepository = GetRequiredService<IOrderRepository>();
    }

    protected override void BeforeAddFakeApplication(IServiceCollection services)
    {
        services.AddTransient<IOrderQueryRepository, OrderQueryRepository>();
    }

    [Fact]
    async Task GetOrderSummaryAsync()
    {
        var orders = await _orderQueryRepository.GetOrderSummaryAsync(AppTestDataBuilder.OrderId);
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
        Should.Throw<FakeException>(async () =>
        {
            await _orderQueryRepository.AddAsync(order);

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
            await _orderQueryRepository.AddBySqlAsync(order);

        }
}