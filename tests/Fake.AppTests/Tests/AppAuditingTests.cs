using System;
using System.Threading.Tasks;
using Domain.Aggregates.OrderAggregate;
using Fake.Modularity;
using Fake.Timing;
using Shouldly;
using Xunit;

namespace Tests;

public abstract class AppAuditingTests<TStartupModule>:AppTestBase<TStartupModule> where TStartupModule : IFakeModule
{
    private Guid _currentUserId;
    private readonly IOrderRepository _orderRepository;
    private readonly IClock _clock;

    public AppAuditingTests()
    {
        _orderRepository = GetRequiredService<IOrderRepository>();
        _clock = GetRequiredService<IClock>();
    }

    [Theory]
    [InlineData("4b2790fc-3f51-43d5-88a1-a92d96a9e6ea")]
    public async Task 创建审计(string currentUserId)
    {
        if (currentUserId != null) _currentUserId = Guid.Parse(currentUserId);
        
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
        var fakeOrder = new Order(currentUserId, "fakeName", new Address(street, city, state, country, zipcode), cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);
        
        Assert.Equal(1, fakeOrder.DomainEvents.Count);

        var orderId = 23;
        fakeOrder.SetId(orderId); 
        await _orderRepository.AddAsync(fakeOrder);
        //await _orderRepository.UnitOfWork.CompleteAsync();

        var order = await _orderRepository.GetAsync(orderId);

        order.ShouldNotBeNull();
        order.CreationTime.ShouldBeLessThanOrEqualTo(_clock.Now);
        order.CreatorId.ShouldBe(_currentUserId);
    }
}