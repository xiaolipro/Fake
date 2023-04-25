using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Repositories;
using Fake.Modularity;
using Shouldly;
using Xunit;

namespace Tests;

public abstract class RepositoryTests<TStartupModule> : AppTestBase<TStartupModule> where TStartupModule : IFakeModule
{
    protected readonly IRepository<Order, Guid> OrderRepository;

    protected RepositoryTests()
    {
        OrderRepository = GetRequiredService<IRepository<Order, Guid>>();
    }
    
    [Fact]
    public async Task GetAsync()
    {
        var order = await OrderRepository.GetAsync(AppTestDataBuilder.OrderId);
        order.OrderItems.Count.ShouldBe(1);
        order.OrderItems.First().GetProductName().ShouldBe("橘子");
    }
    
    [Fact]
    public async Task GetListAsync()
    {
        var orders = await OrderRepository.GetListAsync(o => true, true);
        orders.Count.ShouldBeGreaterThan(0);
        
        orders[0].OrderItems.Count.ShouldBeGreaterThan(0);
    }
}