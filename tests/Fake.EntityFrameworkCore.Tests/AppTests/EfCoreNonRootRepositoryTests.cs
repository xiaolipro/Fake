using Domain.Aggregates.QueriesRepositories;
using Fake;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Shouldly;
using Xunit;

namespace AppTests;

public class EfCoreNonRootRepositoryTests : AppTestBase<FakeEntityFrameworkCoreTestModule>
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
    private void GetOrdersFromUserTest()
    {
        var orders = OrderQueryRepository.GetOrdersFromUserAsync(AppTestDataBuilder.TestUser).Result;
        orders.Count().ShouldBeGreaterThan(0);
        orders.First().ordernumber.ShouldBe(1);
        //orders.First().date.ShouldBe(Clock.Now);
        orders.First().status.ShouldBe("已完成");
        orders.First().total.ShouldBe(100);
    }

    [Fact]
    private async Task UpdateOrdersTest()
    {
        var db = GetRequiredService<OrderingContext>();

        await db.Database.ExecuteSqlRawAsync("select * from orders;update Orders set IsDeleted = false;");
    }

    [Fact]
    private async Task UpdateOrdersTest_Conn()
    {
        var db = GetRequiredService<OrderingContext>();

        await using var cmd = db.Database.GetDbConnection().CreateCommand();
        cmd.CommandText = "select * from orders;update Orders set IsDeleted = false";
        await db.Database.OpenConnectionAsync();

        var res = await cmd.ExecuteNonQueryAsync();
    }
}