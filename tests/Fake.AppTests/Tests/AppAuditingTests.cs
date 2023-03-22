using System;
using System.Threading.Tasks;
using Domain.Aggregates.OrderAggregate;
using Fake.Identity.Users;
using Fake.Modularity;
using Fake.Timing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Tests;

public abstract class AppAuditingTests<TStartupModule>:AppTestBase<TStartupModule> where TStartupModule : IFakeModule
{
    private Guid CurrentUserId;
    private readonly IOrderRepository _orderRepository;
    private readonly IClock _clock;

    public AppAuditingTests()
    {
        _orderRepository = GetRequiredService<IOrderRepository>();
        _clock = GetRequiredService<IClock>();
    }

    protected override void AfterAddFakeApplication(IServiceCollection services)
    {
        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.UserId.Returns(ci => CurrentUserId.ToString());

        services.AddSingleton(currentUser);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("4b2790fc-3f51-43d5-88a1-a92d96a9e6ea")]
    public async Task 创建审计(string currentUserId)
    {
        if (currentUserId != null) CurrentUserId = Guid.Parse(currentUserId);
        
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

        Guid orderId = default;
        await WithUnitOfWorkAsync(async () =>
        {
            var order = await _orderRepository.AddAsync(fakeOrder);
            orderId = order.Id;
            //await _orderRepository.UnitOfWork.CompleteAsync();
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var order = await _orderRepository.GetAsync(orderId);

            order.ShouldNotBeNull();
            order.CreationTime.ShouldBeLessThanOrEqualTo(_clock.Now);
            order.CreatorId.ShouldBe(CurrentUserId);
        });
    }
}