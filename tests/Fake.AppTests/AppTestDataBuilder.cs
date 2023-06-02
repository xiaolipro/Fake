using System;
using System.Threading.Tasks;
using Domain.Aggregates.BuyerAggregate;
using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Repositories;

public class AppTestDataBuilder
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Buyer> _buyerRepository;

    public static Guid UserId = new("1fcf46b2-28c3-48d0-8bac-fa53268a2775");
    public static Guid OrderId = new("4d734a0e-3e6b-4bad-bb43-ef8cf1b09633");
    public static Guid PaymentMethodId = new("5d734a0e-3e6b-4bad-bb43-ef8cf1b09633");
    

    public AppTestDataBuilder(IRepository<Order> orderRepository, IRepository<Buyer> buyerRepository)
    {
        _orderRepository = orderRepository;
        _buyerRepository = buyerRepository;
    }

    public async Task BuildAsync()
    {
        await AddPaymentMethod();
        await AddOrder();
    }

    private async Task AddPaymentMethod()
    {
        var buyer = new Buyer(UserId, "fake");
        buyer.AddPaymentMethod(1, "fakeAlias", "fakeCardNumber",
            "fakeSecurityNumber", "fakeCardHolderName",
            DateTime.Now.AddYears(1), OrderId);
        await _buyerRepository.InsertAsync(buyer);
    }

    private async Task AddOrder()
    {
        var street = "fakeStreet";
        var city = "FakeCity";
        var state = "fakeState";
        var country = "fakeCountry";
        var zipcode = "FakeZipCode";
        var cardTypeId = 5;
        var cardNumber = "12";
        var cardSecurityNumber = "123";
        var cardHolderName = "FakeName";
        var cardExpiration = DateTime.Now.AddYears(1);
        var order = new Order(UserId, "fakeName", new Address(street, city, state, country, zipcode),
            cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);

        order.AddOrderItem(1, "橘子", 9.6m, 9, null);
        order.SetId(OrderId);
        order.SetBuyerId(UserId);
        order.SetPaymentId(PaymentMethodId);
        order.SetPaidStatus();
        await _orderRepository.InsertAsync(order);
    }
}