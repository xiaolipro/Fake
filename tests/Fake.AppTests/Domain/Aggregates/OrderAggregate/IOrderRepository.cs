using System.Threading.Tasks;

namespace Domain.Aggregates.OrderAggregate;

public interface IOrderRepository
{
    Order Add(Order order);

    void Update(Order order);

    Task<Order> GetAsync(int orderId);
}