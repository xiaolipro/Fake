﻿using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Repositories.EntityFrameWorkCore;
using Microsoft.EntityFrameworkCore;

namespace AppTests.Repositories;

public class OrderRepository : EfCoreRepository<OrderingContext, Order, Guid>, IOrderRepository
{
    public OrderRepository(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public async Task<Order> AddAsync(Order order)
    {
        var context = await GetDbContextAsync();
        var entry = await context.Orders.AddAsync(order);
        return entry.Entity;
    }

    public void Update(Order order)
    {
        throw new NotImplementedException();
    }

    public async Task<Order> GetAsync(Guid orderId)
    {
        var context = await GetDbContextAsync();
        var order = await context
            .Orders
            .Include(x => x.Address)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
        {
            order = context
                .Orders
                .Local
                .FirstOrDefault(o => o.Id == orderId);
        }

        if (order != null)
        {
            await context.Entry(order)
                .Collection(i => i.OrderItems).LoadAsync();
            await context.Entry(order)
                .Reference(i => i.OrderStatus).LoadAsync();
        }

        return order;
    }
}