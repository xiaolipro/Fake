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
    void GetOrdersFromUserTest()
    {
        var orders = OrderQueryRepository.GetOrdersFromUserAsync(AppTestDataBuilder.TestUser).Result;
        orders.Count().ShouldBeGreaterThan(0);
        orders.First().ordernumber.ShouldBe(1);
        //orders.First().date.ShouldBe(Clock.Now);
        orders.First().status.ShouldBe("已完成");
        orders.First().total.ShouldBe(100);
    }
}