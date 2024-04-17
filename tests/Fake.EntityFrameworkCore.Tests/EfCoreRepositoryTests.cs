using Domain.Aggregates.OrderAggregate;
using Fake.DomainDrivenDesign.Repositories.EntityFrameWorkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Fake.EntityFrameworkCore.Tests;

public class EfCoreRepositoryTests : AppTestBase<FakeEntityFrameworkCoreTestModule>
{
    protected readonly IEfCoreRepository<OrderingContext, Order> OrderRepository;

    public EfCoreRepositoryTests()
    {
        OrderRepository = ServiceProvider.GetRequiredService<IEfCoreRepository<OrderingContext, Order>>();
    }

    [Fact]
    public async Task GetWithNavAsync()
    {
        var order = await OrderRepository.FirstOrDefaultAsync(x => x.Id == AppTestDataBuilder.OrderId);
        order.ShouldNotBeNull();
        order.OrderItems.Count.ShouldBeGreaterThan(0);
    }
}