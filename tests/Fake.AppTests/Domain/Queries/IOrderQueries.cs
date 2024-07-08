using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Repositories;

namespace Domain.Queries;

public interface IOrderQueries : IRootlessRepository
{
    Task<List<OrderSummary>> GetOrderSummaryAsync(Guid userId);

    Task<Order> AddAsync(Order order);

    Task AddBySqlAsync(Order order);
}

public class OrderSummary
{
    public Guid ordernumber { get; set; }
    public DateTime date { get; set; }
    public OrderStatus status { get; set; }
    public double total { get; set; }
}