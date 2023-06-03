using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Repositories;

namespace Domain.Aggregates.QueriesRepositories;

public interface IOrderQueryRepository:INoRootRepository
{
    Task<List<OrderSummary>> GetOrderSummaryAsync(Guid userId);
}

public class OrderSummary
{
    public Guid ordernumber { get; set; }
    public DateTime date { get; set; }
    public OrderStatus status { get; set; }
    public double total { get; set; }
}