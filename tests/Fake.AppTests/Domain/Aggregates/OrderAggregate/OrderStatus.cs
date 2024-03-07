using Fake.DomainDrivenDesign;

namespace Domain.Aggregates.OrderAggregate;

public class OrderStatus(string name, int value) : Enumeration(name, value)
{
    public static OrderStatus Submitted = new OrderStatus(nameof(Submitted).ToLowerInvariant(), 1);
    public static OrderStatus AwaitingValidation = new OrderStatus(nameof(AwaitingValidation).ToLowerInvariant(), 2);
    public static OrderStatus StockConfirmed = new OrderStatus(nameof(StockConfirmed).ToLowerInvariant(), 3);
    public static OrderStatus Paid = new OrderStatus(nameof(Paid).ToLowerInvariant(), 4);
    public static OrderStatus Shipped = new OrderStatus(nameof(Shipped).ToLowerInvariant(), 5);
    public static OrderStatus Cancelled = new OrderStatus(nameof(Cancelled).ToLowerInvariant(), 6);
}