using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Repositories;
using Fake.Modularity;
using Xunit;

namespace Tests;

public class VersionTests<TStartupModule> : AppTestBase<TStartupModule>
    where TStartupModule : IFakeModule
{
    protected readonly IRepository<Order> OrderRepository;

    protected VersionTests()
    {
        OrderRepository = GetRequiredService<IRepository<Order>>();
    }
    
    // [Fact]
    // void 脏读不会被保存()
    // {
    //     var order = OrderRepository.FirstOrDefaultAsync(x => x)
    // }
}