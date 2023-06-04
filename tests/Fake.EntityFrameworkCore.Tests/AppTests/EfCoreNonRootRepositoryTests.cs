using Domain.Aggregates.OrderAggregate;
using Domain.Aggregates.QueriesRepositories;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Shouldly;
using Xunit;

namespace AppTests;

public class EfCoreNonRootRepositoryTests: AppTestBase<FakeEntityFrameworkCoreTestModule>
{
    private IOrderQueryRepository OrderQueryRepository;
    public EfCoreNonRootRepositoryTests()
    {
        OrderQueryRepository = GetRequiredService<IOrderQueryRepository>();
    }

    protected override void BeforeAddFakeApplication(IServiceCollection services)
    {
        services.AddTransient<IOrderQueryRepository, OrderQueryRepository>();
    }

    [Fact]
    async Task GetOrderSummaryAsync()
    {
        var orders = await OrderQueryRepository.GetOrderSummaryAsync(AppTestDataBuilder.OrderId);
        orders.Count.ShouldBe(1);
        orders.First().date.ShouldBeLessThanOrEqualTo(FakeClock.Now);
        orders[0].status.ShouldBe(OrderStatus.Submitted.Name);
        orders[0].total.ShouldBe(20.4);
    }
}