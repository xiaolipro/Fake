using System;
using System.Threading.Tasks;
using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Repositories;
using Fake.Identity.Users;
using Fake.Modularity;
using Fake.Timing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Xunit;
using static System.Guid;

namespace Tests;

public abstract class AppAuditingTests<TStartupModule> : AppTestBase<TStartupModule> where TStartupModule : IFakeModule
{
    private Guid _currentUserId;
    private readonly IRepository<Order, Guid> _orderRepository;
    private readonly IClock _clock;

    public AppAuditingTests()
    {
        _orderRepository = GetRequiredService<IRepository<Order, Guid>>();
        _clock = GetRequiredService<IClock>();
    }

    protected override void AfterAddFakeApplication(IServiceCollection services)
    {
        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.UserId.Returns(ci => _currentUserId.ToString());

        services.AddSingleton(currentUser);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("4b2790fc-3f51-43d5-88a1-a92d96a9e6ea")]
    public async Task 创建审计(string currentUserId)
    {
        TryParse(currentUserId, out _currentUserId);

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
        var fakeOrder = new Order(currentUserId, "fakeName", new Address(street, city, state, country, zipcode),
            cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);

        Assert.Equal(1, fakeOrder.DomainEvents.Count);

        var order = await _orderRepository.InsertAsync(fakeOrder);

        order = await _orderRepository.GetFirstOrNullAsync(order.Id);

        order.ShouldNotBeNull();
        order.CreationTime.ShouldBeLessThanOrEqualTo(_clock.Now);
        order.CreatorId.ShouldBe(_currentUserId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("4b2790fc-3f51-43d5-88a1-a92d96a9e6ea")]
    public async Task 修改审计(string currentUserId)
    {
        TryParse(currentUserId, out _currentUserId);

        var order = await _orderRepository.GetFirstOrNullAsync(AppTestDataBuilder.OrderId);
        order.LastModifierId.ShouldBe(Empty);

        order.SetCancelledStatus();
        order = await _orderRepository.UpdateAsync(order);

        order.LastModifierId.ShouldBe(_currentUserId);
        order.LastModificationTime.ShouldBeLessThanOrEqualTo(_clock.Now);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("4b2790fc-3f51-43d5-88a1-a92d96a9e6ea")]
    public async Task 软删审计(string currentUserId)
    {
        TryParse(currentUserId, out _currentUserId);

        var order = await _orderRepository.GetFirstOrNullAsync(AppTestDataBuilder.OrderId);
        await _orderRepository.DeleteAsync(order);

        order = await _orderRepository.GetFirstOrNullAsync(AppTestDataBuilder.OrderId);
        order.ShouldBeNull();
    }
}