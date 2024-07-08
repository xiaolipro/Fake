using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Repositories;
using Fake.Modularity;
using Fake.UnitOfWork;
using Xunit;

namespace Tests;

public abstract class UowTests<TStartupModule> : AppTestBase<TStartupModule>
    where TStartupModule : IFakeModule
{
    protected readonly IRepository<Order> OrderRepository;
    protected readonly IUnitOfWorkManager UowManager;


    protected UowTests()
    {
        OrderRepository = ServiceProvider.GetRequiredService<IRepository<Order>>();
        UowManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
    }


    [Fact]
    public async Task ChildUow共享CurrentUow的上下文()
    {
        using var uow = UowManager.Begin();
        await OrderRepository.FirstAsync(x => x.Id == AppTestDataBuilder.OrderId);
        await OrderRepository.FirstAsync(x => x.Id == AppTestDataBuilder.OrderId);
    }
}