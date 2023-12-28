using System.Diagnostics;
using Domain.Aggregates.OrderAggregate;
using Fake.Data.Filtering;
using Fake.DomainDrivenDesign.Entities.Auditing;
using Fake.DomainDrivenDesign.Repositories;
using Fake.Modularity;
using Shouldly;
using Xunit;

namespace Tests;

public abstract class DataFilterTests<TStartupModule> : AppTestBase<TStartupModule> where TStartupModule : IFakeModule
{
    protected readonly IDataFilter<ISoftDelete> SoftDeleteDataFilter;
    protected readonly IRepository<Order> OrderRepository;

    public DataFilterTests()
    {
        OrderRepository = GetRequiredService<IRepository<Order>>();
        SoftDeleteDataFilter = GetRequiredService<IDataFilter<ISoftDelete>>();
    }

    [Fact]
    public async Task 关闭软删审计()
    {
        await SoftDeleteAsync();

        using (var _ = SoftDeleteDataFilter.Disable())
        {
            var order = await OrderRepository.FirstOrDefaultAsync(x => x.Id == AppTestDataBuilder.OrderId);
            order.ShouldNotBeNull();
        }
    }


    private async Task SoftDeleteAsync()
    {
        var order = await OrderRepository.FirstOrDefaultAsync(x => x.Id == AppTestDataBuilder.OrderId);
        order.ShouldNotBeNull();
        order.CreationTime.ShouldBeLessThanOrEqualTo(FakeClock.Now);
        order.LastModifierId.ShouldBe(Guid.Empty);
        await OrderRepository.DeleteAsync(order);

        order.IsDeleted.ShouldBe(true);
        order.LastModifierId.ShouldBe(Guid.Empty);
        Debug.Assert(order.LastModificationTime != null, "order.LastModificationTime != null");
        order.LastModificationTime.Value.ShouldBeLessThanOrEqualTo(FakeClock.Now);
    }
}