using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Repositories.EntityFrameWorkCore;

namespace Fake.EntityFrameworkCore.Tests.AppTests.Repositories;

public class OrderRepository:EfCoreRepository<AppDbContext>, IOrderRepository
{

    public OrderRepository(IDbContextProvider<AppDbContext> dbContextProvider) : base(dbContextProvider)
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

    public Task<Order> GetAsync(int orderId)
    {
        throw new NotImplementedException();
    }
}