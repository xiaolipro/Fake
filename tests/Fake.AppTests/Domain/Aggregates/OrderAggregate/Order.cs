using Domain.Aggregates.BuyerAggregate;
using Domain.Events;
using Fake.Auditing;
using Fake.Domain;
using Fake.Domain.Entities.Auditing;

namespace Domain.Aggregates.OrderAggregate;

[Audited]
public class Order : FullAuditedAggregate<Guid, Guid>
{
    // DDD Patterns comment
    // Using private fields, allowed since EF Core 1.1, is a much better encapsulation
    // aligned with DDD Aggregates and Domain Entities (Instead of properties and property collections)
    private DateTime _orderDate;

    // Address is a Value Object pattern example persisted as EF Core 2.0 owned entity
    public Address Address { get; private set; }

    public Guid? GetBuyerId => _buyerId;
    private Guid? _buyerId;

    public OrderStatus OrderStatus { get; private set; }

    private string _description;


    // Draft orders have this set to true. Currently we don't check anywhere the draft status of an Order, but we could do it if needed
#pragma warning disable CS0414
    private bool _isDraft;
#pragma warning restore CS0414

    // DDD Patterns comment
    // Using a private collection field, better for DDD Aggregate's encapsulation
    // so OrderItems cannot be added from "outside the AggregateRoot" directly to the collection,
    // but only through the method OrderAggrergateRoot.AddOrderItem() which includes behaviour.
    private readonly List<OrderItem> _orderItems;
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    private Guid? _paymentMethodId;

    public static Order NewDraft()
    {
        var order = new Order();
        order._isDraft = true;
        return order;
    }

    protected Order()
    {
        _orderItems = new List<OrderItem>();
        _isDraft = false;
    }

    public Order(Guid userId, string userName, Address address, CardType cardType, string cardNumber,
        string cardSecurityNumber,
        string cardHolderName, DateTime cardExpiration, Guid? buyerId = null, Guid? paymentMethodId = null) : this()
    {
        _buyerId = buyerId;
        _paymentMethodId = paymentMethodId;
        OrderStatus = OrderStatus.Submitted;
        Address = address;

        // Add the OrderStarterDomainEvent to the domain events collection 
        // to be raised/dispatched when comitting changes into the Database [ After DbContext.SaveChanges() ]
        var orderStartedDomainEvent = new OrderStartedDomainEvent(this, userId, userName, cardType,
            cardNumber, cardSecurityNumber,
            cardHolderName, cardExpiration);

        this.AddDomainEvent(orderStartedDomainEvent);
    }

    // DDD Patterns comment
    // This Order AggregateRoot's method "AddOrderitem()" should be the only way to add Items to the Order,
    // so any behavior (discounts, etc.) and validations are controlled by the AggregateRoot 
    // in order to maintain consistency between the whole Aggregate. 
    public void AddOrderItem(int productId, string productName, decimal unitPrice, decimal discount, string pictureUrl,
        int units = 1)
    {
        var existingOrderForProduct = _orderItems.Where(o => o.ProductId == productId)
            .SingleOrDefault();

        if (existingOrderForProduct != null)
        {
            //if previous line exist modify it with higher discount  and units..

            if (discount > existingOrderForProduct.GetCurrentDiscount())
            {
                existingOrderForProduct.SetNewDiscount(discount);
            }

            existingOrderForProduct.AddUnits(units);
        }
        else
        {
            //add validated new order item

            var orderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);
            _orderItems.Add(orderItem);
        }
    }

    public void SetPaymentId(Guid id)
    {
        _paymentMethodId = id;
    }

    public void SetBuyerId(Guid id)
    {
        _buyerId = id;
    }

    public void SetAwaitingValidationStatus()
    {
        if (OrderStatus == OrderStatus.Submitted)
        {
            AddDomainEvent(new OrderStatusChangedToAwaitingValidationDomainEvent(Id, _orderItems));
            OrderStatus = OrderStatus.AwaitingValidation;
        }
    }

    public void SetStockConfirmedStatus()
    {
        if (OrderStatus == OrderStatus.AwaitingValidation)
        {
            AddDomainEvent(new OrderStatusChangedToStockConfirmedDomainEvent(Id));

            OrderStatus = OrderStatus.StockConfirmed;
            _description = "All the items were confirmed with available stock.";
        }
    }

    public void SetPaidStatus()
    {
        if (OrderStatus == OrderStatus.StockConfirmed)
        {
            AddDomainEvent(new OrderStatusChangedToPaidDomainEvent(Id, OrderItems));

            OrderStatus = OrderStatus.Paid;
            _description =
                "The payment was performed at a simulated \"American Bank checking bank account ending on XX35071\"";
        }
    }

    public void SetShippedStatus()
    {
        if (OrderStatus != OrderStatus.Paid)
        {
            StatusChangeException(OrderStatus.Shipped);
        }

        OrderStatus = OrderStatus.Shipped;
        _description = "The order was shipped.";
        AddDomainEvent(new OrderShippedDomainEvent(this));
    }

    public void SetCancelledStatus()
    {
        if (OrderStatus == OrderStatus.Paid ||
            OrderStatus == OrderStatus.Shipped)
        {
            StatusChangeException(OrderStatus.Cancelled);
        }

        OrderStatus = OrderStatus.Cancelled;
        _description = $"The order was cancelled.";
        AddDomainEvent(new OrderCancelledDomainEvent(this));
    }

    public void SetCancelledStatusWhenStockIsRejected(IEnumerable<int> orderStockRejectedItems)
    {
        if (OrderStatus == OrderStatus.AwaitingValidation)
        {
            OrderStatus = OrderStatus.Cancelled;

            var itemsStockRejectedProductNames = OrderItems
                .Where(c => orderStockRejectedItems.Contains(c.ProductId))
                .Select(c => c.GetOrderItemProductName());

            var itemsStockRejectedDescription = string.Join(", ", itemsStockRejectedProductNames);
            _description = $"The product items don't have stock: ({itemsStockRejectedDescription}).";
        }
    }

    private void StatusChangeException(OrderStatus orderStatusToChange)
    {
        throw new DomainException(
            $"Is not possible to change the order status from {OrderStatus.Name} to {orderStatusToChange.Name}.");
    }

    public decimal GetTotal()
    {
        return _orderItems.Sum(o => o.GetUnits() * o.GetUnitPrice());
    }

    public void SetDescription(string description)
    {
        _description = description;
    }
}