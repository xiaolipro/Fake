using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Aggregates.OrderAggregate;
using Fake.DomainDrivenDesign.Repositories;

namespace Domain.Queries;

public interface IOrderQueries : INoRootRepository
{
    Task<List<OrderSummary>> GetOrderSummaryAsync(Guid userId);

    Task<Order> AddAsync(Order order);

    Task AddBySqlAsync(Order order);
}

public class OrderSummary
{
    public Guid ordernumber { get; set; }
    public DateTime date { get; set; }
    public string status { get; set; }
    public double total { get; set; }
}