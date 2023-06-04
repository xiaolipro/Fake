using System;
using System.Threading.Tasks;
using Domain.Aggregates.BuyerAggregate;
using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Repositories;
using Fake.Identity.Users;
using Fake.Modularity;
using Fake.Timing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Tests;

public abstract class AppAuditingTests<TStartupModule> : AppTestBase<TStartupModule> where TStartupModule : IFakeModule
{
    private Guid _currentUserId;
    private readonly IRepository<Order> _orderRepository;

    public AppAuditingTests()
    {
        _orderRepository = GetRequiredService<IRepository<Order>>();
    }

    protected override void AfterAddFakeApplication(IServiceCollection services)
    {
        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.UserId.Returns(_ => _currentUserId.ToString());

        services.AddSingleton(currentUser);
    }

    [Fact]
    public async Task 创建审计()
    {
        var street = "fakeStreet";
        var city = "FakeCity";
        var state = "fakeState";
        var country = "fakeCountry";
        var zipcode = "FakeZipCode";
        var cardType = CardType.Amex;
        var cardNumber = "12";
        var cardSecurityNumber = "123";
        var cardHolderName = "FakeName";
        var cardExpiration = FakeClock.Now.AddYears(1);
        var fakeOrder = new Order(AppTestDataBuilder.UserId, "fakeName", new Address(street, city, state, country, zipcode),
            cardType, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);

        Assert.Equal(1, fakeOrder.DomainEvents.Count);

        var order = await _orderRepository.AddAsync(fakeOrder);

        order = await _orderRepository.FirstOrDefaultAsync(x => x.Id == order.Id);

        order.ShouldNotBeNull();
        order.CreationTime.ShouldBeLessThanOrEqualTo(FakeClock.Now);
        order.CreatorId.ShouldBe(_currentUserId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("4b2790fc-3f51-43d5-88a1-a92d96a9e6ea")]
    public async Task 修改审计(string currentUserId)
    {
        Guid.TryParse(currentUserId, out _currentUserId);

        var order = await _orderRepository.FirstOrDefaultAsync(x => x.Id == AppTestDataBuilder.OrderId);
        order.LastModifierId.ShouldBe(Guid.Empty);

        order.SetCancelledStatus();
        order = await _orderRepository.UpdateAsync(order);

        order.LastModifierId.ShouldBe(_currentUserId);
        order.LastModificationTime.ShouldBeLessThanOrEqualTo(FakeClock.Now);
    }

    [Theory]
    [InlineData("4b2790fc-3f51-43d5-88a1-a92d96a9e6ea")]
    public async Task 软删审计(string currentUserId)
    {
        Guid.TryParse(currentUserId, out _currentUserId);

        var order = await _orderRepository.FirstOrDefaultAsync(x => x.Id ==AppTestDataBuilder.OrderId);
        order.CreationTime.ShouldBeLessThanOrEqualTo(FakeClock.Now);
        order.LastModifierId.ShouldBe(Guid.Empty);
        await _orderRepository.DeleteAsync(order);

        order.IsDeleted.ShouldBe(true);
        order.LastModifierId.ShouldBe(_currentUserId);
        order.LastModificationTime.ShouldBeLessThanOrEqualTo(FakeClock.Now);

        //TODO：被删除（软）的数据不应该被查询到（默认情况下）
        order = await _orderRepository.FirstOrDefaultAsync(x => x.Id ==AppTestDataBuilder.OrderId);
        order.ShouldBeNull();
    }
}