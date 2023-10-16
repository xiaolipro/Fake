using Domain.Aggregates.OrderAggregate;
using Domain.Aggregates.QueriesRepositories;
using Fake.Domain.Repositories.EntityFrameWorkCore;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class OrderQueryRepository : EfCoreNoRootRepository<OrderingContext>, IOrderQueryRepository
{
    public async Task<List<OrderSummary>> GetOrderSummaryAsync(Guid orderId)
    {
        var context = await GetDbContextAsync();
        
        var res = context.Database.SqlQuery<OrderSummary>(
            @$"SELECT o.[Id] as ordernumber,o.[CreationTime] as [date],os.[Name] as [status], SUM(oi.units*oi.unitprice) as total
               FROM orders o
               LEFT JOIN orderitems oi ON  o.Id = oi.orderid 
               LEFT JOIN orderstatus os on o.OrderStatusId = os.Id
               Where o.[Id] = {orderId}
               GROUP BY o.[Id], o.[CreationTime], os.[Name] 
               ORDER BY o.[Id]")
            .ToList();

        return res;
    }
    
    
    public async Task<Order> AddAsync(Order order)
    {
        var context = await GetDbContextAsync();
        var entry = context.Orders.Add(order);
        return entry.Entity;
    }

    public async Task AddBySqlAsync(Order order)
    {
        var context = await GetDbContextAsync();
        
        await context.Database.ExecuteSqlAsync(@$"CREATE TABLE students (
                                    id INT PRIMARY KEY,
                                    name VARCHAR(50),
                                    age INT,
                                    gender VARCHAR(10),
                                    grade VARCHAR(10)
                                   )");
    }
}