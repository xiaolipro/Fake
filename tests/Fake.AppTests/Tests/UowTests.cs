using System.Linq;
using System.Threading.Tasks;
using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Repositories;
using Fake.Modularity;
using Fake.UnitOfWork;
using Shouldly;
using Xunit;

namespace Tests;

public abstract class UowTests<TStartupModule> : AppTestBase<TStartupModule>
    where TStartupModule : IFakeModule
{
    protected readonly IRepository<Order> OrderRepository;
    protected readonly IUnitOfWorkManager UowManager;
    

    protected UowTests()
    {
        OrderRepository = GetRequiredService<IRepository<Order>>();
        UowManager = GetRequiredService<IUnitOfWorkManager>();
    }


    [Fact]
    public async Task ChildUow共享CurrentUow的上下文()
    {
        using var uow = UowManager.Begin();
        var order = await OrderRepository.FirstOrDefaultAsync(x => x.Id == AppTestDataBuilder.OrderId);
        order.OrderItems.Count.ShouldBe(2);
        order.OrderItems.First().GetProductName().ShouldBe("橘子");
        
        
        var order2 = await OrderRepository.FirstOrDefaultAsync(x => x.Id == AppTestDataBuilder.OrderId);
        order2.OrderItems.Count.ShouldBe(2);
        order2.OrderItems.First().GetProductName().ShouldBe("橘子");
    }
}