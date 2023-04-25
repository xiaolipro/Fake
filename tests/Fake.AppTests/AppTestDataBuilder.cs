using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Repositories;

public class AppTestDataBuilder
{
    private readonly IRepository<Order, Guid> _orderRepository;

    public static Guid TestUser = new("1fcf46b2-28c3-48d0-8bac-fa53268a2775");
    public static Guid OrderId = new("4d734a0e-3e6b-4bad-bb43-ef8cf1b09633");

    public AppTestDataBuilder(IRepository<Order, Guid> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task BuildAsync()
    {
        await AddOrder();
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
        var order = new Order(TestUser.ToString(), "fakeName", new Address(street, city, state, country, zipcode),
            cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);

        order.AddOrderItem(1, "橘子", 9.6m, 9, null);
        order.SetId(OrderId);
        await _orderRepository.InsertAsync(order);
    }
}