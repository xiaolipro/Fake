using Domain.Aggregates.OrderAggregate;
using Fake.DomainDrivenDesign.Repositories;
using Fake.Modularity;
using Shouldly;
using Xunit;

namespace Tests;

public abstract class RepositoryTests<TStartupModule> : AppTestBase<TStartupModule> where TStartupModule : IFakeModule
{
    protected readonly IRepository<Order> OrderRepository;

    protected RepositoryTests()
    {
        OrderRepository = ServiceProvider.GetRequiredService<IRepository<Order>>();
    }

    [Fact]
    public async Task GetAsync()
    {
        var order = await OrderRepository.FirstOrDefaultAsync(x => x.Id == AppTestDataBuilder.OrderId);
        order!.OrderItems.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetListAsync()
    {
        var orders = await OrderRepository.GetListAsync();
        orders.Count.ShouldBeGreaterThan(0);

        orders[0].OrderItems.Count.ShouldBeGreaterThan(0);
    }
}