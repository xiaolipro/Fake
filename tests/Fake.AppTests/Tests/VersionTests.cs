﻿using Domain.Aggregates.OrderAggregate;
using Fake.Data;
using Fake.Domain.Repositories;
using Fake.Modularity;
using Xunit;

namespace Tests;

public abstract class VersionTests<TStartupModule> : AppTestBase<TStartupModule>
    where TStartupModule : IFakeModule
{
    protected readonly IRepository<Order?> OrderRepository;

    protected VersionTests()
    {
        OrderRepository = GetRequiredService<IRepository<Order>>();
    }

    [Fact]
    public async Task 脏读不会被更新()
    {
        var order = await OrderRepository.FirstOrDefaultAsync(x => x.Id == AppTestDataBuilder.OrderId);
        order.SetDescription("hello");

        var order2 = await OrderRepository.FirstOrDefaultAsync(x => x.Id == AppTestDataBuilder.OrderId);
        order2.SetDescription("ok");
        await OrderRepository.UpdateAsync(order);
        //And updating my old entity throws exception!
        await Assert.ThrowsAsync<FakeDbConcurrencyException>(async () =>
        {
            await OrderRepository.UpdateAsync(order2);
        });
    }

    [Fact]
    public async Task 脏读不会被删除()
    {
        var order = await OrderRepository.FirstOrDefaultAsync(x => x.Id == AppTestDataBuilder.OrderId);

        var order2 = await OrderRepository.FirstOrDefaultAsync(x => x.Id == AppTestDataBuilder.OrderId);
        order2.SetDescription("hello");
        await OrderRepository.UpdateAsync(order2);
        //And updating my old entity throws exception!
        await Assert.ThrowsAsync<FakeDbConcurrencyException>(async () => { await OrderRepository.DeleteAsync(order); });
    }
}