using Domain.Aggregates.OrderAggregate;
using Fake.DomainDrivenDesign.Repositories.EntityFrameWorkCore;
using Microsoft.EntityFrameworkCore;

namespace Domain.Queries;

public class OrderQueries : EfCoreNoRootRepository<OrderingContext>, IOrderQueries
{
    public async Task<List<OrderSummary>> GetOrderSummaryAsync(Guid orderId)
    {
        var context = await GetDbContextAsync();

        var res = context.Database.SqlQuery<OrderSummary>(
                @$"SELECT o.[Id] as ordernumber,o.[CreationTime] as [date],os.[Name] as [status], SUM(oi.units*oi.unitprice) as total
               FROM orders o
               LEFT JOIN orderitems oi ON  o.Id = oi.orderid 
               LEFT JOIN orderstatus os on o.OrderStatus = os.Id
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

        await context.Database.ExecuteSqlAsync($"INSERT INTO \"cardtypes\" (\"Name\")VALUES ('Alipay');");
    }
}