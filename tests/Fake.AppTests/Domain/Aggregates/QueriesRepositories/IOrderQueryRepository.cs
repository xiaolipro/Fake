using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fake.Domain.Repositories;

namespace Domain.Aggregates.QueriesRepositories;

public interface IOrderQueryRepository:INoRootRepository
{
    Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(Guid userId);
}

public class OrderSummary
{
    public int ordernumber { get; set; }
    public DateTime date { get; set; }
    public string status { get; set; }
    public double total { get; set; }
}