using Domain.Aggregates.QueriesRepositories;
using Fake.Domain.Repositories.EntityFrameWorkCore;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class OrderQueryRepository:EfCoreNoRootRepository<OrderingContext>,IOrderQueryRepository
{
    public async Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(Guid userId)
    {
        var context = await GetDbContextAsync();
        
        return context.Database.SqlQuery<OrderSummary>(
            $@"SELECT o.[Id] as ordernumber,o.[OrderDate] as [date],os.[Name] as [status], SUM(oi.units*oi.unitprice) as total
                     FROM [Orders] o
                     LEFT JOIN [orderitems] oi ON  o.Id = oi.orderid 
                     LEFT JOIN [orderstatus] os on o.OrderStatusId = os.Id                     
                     LEFT JOIN [buyers] ob on o.BuyerId = ob.Id
                     WHERE ob.IdentityGuid = '1fcf46b2-28c3-48d0-8bac-fa53268a2775'
                     GROUP BY o.[Id], o.[OrderDate], os.[Name] 
                     ORDER BY o.[Id]")
            .ToList();
    }
}