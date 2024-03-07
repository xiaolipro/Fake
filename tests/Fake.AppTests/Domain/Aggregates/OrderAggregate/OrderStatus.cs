namespace Domain.Aggregates.OrderAggregate;

public enum OrderStatus
{
    Submitted = 1,
    AwaitingValidation,
    StockConfirmed,
    Paid,
    Shipped,
    Cancelled,
}