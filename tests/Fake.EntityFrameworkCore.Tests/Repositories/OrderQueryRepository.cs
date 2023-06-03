using Domain.Aggregates.QueriesRepositories;
using Fake.Domain.Repositories.EntityFrameWorkCore;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class OrderQueryRepository:EfCoreNoRootRepository<OrderingContext>,IOrderQueryRepository
{
    public async Task<List<OrderSummary>> GetOrderSummaryAsync(Guid orderId)
    {
        var context = await GetDbContextAsync();
        
        return context.Database.SqlQuery<OrderSummary>(
            $@"SELECT o.[Id] as ordernumber,o.[OrderDate] as [date],os.[Name] as [status], SUM(oi.units*oi.unitprice) as total
                     FROM orders o
                     LEFT JOIN orderitems oi ON  o.Id = oi.orderid 
                     LEFT JOIN orderstatus os on o.OrderStatusId = os.Id    
                     WHERE o.Id = '{orderId}'
                     GROUP BY o.[Id], o.[OrderDate], os.[Name] 
                     ORDER BY o.[Id]")
            .ToList();
    }
}