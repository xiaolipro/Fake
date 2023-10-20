using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Domain.Aggregates.BuyerAggregate;
using Domain.Aggregates.OrderAggregate;
using Fake.Auditing;
using Fake.Domain.Repositories;
using Fake.Identity.Users;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Tests;

public abstract class AppAuditingTests<TStartupModule> : AppTestBase<TStartupModule> where TStartupModule : IFakeModule
{
    protected Guid CurrentUserId;
    protected readonly IRepository<Order> OrderRepository;
    protected readonly IAuditingManager AuditingManager;

    public AppAuditingTests()
    {
        OrderRepository = GetRequiredService<IRepository<Order>>();
        AuditingManager = GetRequiredService<IAuditingManager>();
    }

    protected override void AfterAddFakeApplication(IServiceCollection services)
    {
        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.UserId.Returns(_ => CurrentUserId.ToString());

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
        var fakeOrder = new Order(AppTestDataBuilder.UserId, "fakeName",
            new Address(street, city, state, country, zipcode),
            cardType, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);

        Assert.Equal(1, fakeOrder.DomainEvents.Count);

        var order = await OrderRepository.InsertAsync(fakeOrder);

        order = await OrderRepository.FirstOrDefaultAsync(x => x.Id == order.Id);

        order.ShouldNotBeNull();
        order.CreationTime.ShouldBeLessThanOrEqualTo(FakeClock.Now);
        order.CreatorId.ShouldBe(CurrentUserId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("4b2790fc-3f51-43d5-88a1-a92d96a9e6ea")]
    public async Task 修改审计(string currentUserId)
    {
        using var scope = AuditingManager.BeginScope();
        Guid.TryParse(currentUserId, out CurrentUserId);

        var order = await OrderRepository.FirstOrDefaultAsync(x => x.Id == AppTestDataBuilder.OrderId);
        order.LastModifierId.ShouldBe(Guid.Empty);

        order.SetCancelledStatus();
        order.SetDescription("ok");
        order = await OrderRepository.UpdateAsync(order);

        order.LastModifierId.ShouldBe(CurrentUserId);
        Debug.Assert(order.LastModificationTime != null, "order.LastModificationTime != null");
        order.LastModificationTime.Value.ShouldBeLessThanOrEqualTo(FakeClock.Now);

        await scope.SaveAsync();
    }

    [Theory]
    [InlineData("4b2790fc-3f51-43d5-88a1-a92d96a9e6ea")]
    public async Task 软删审计(string currentUserId)
    {
        Guid.TryParse(currentUserId, out CurrentUserId);

        var order = await OrderRepository.FirstOrDefaultAsync(x => x.Id == AppTestDataBuilder.OrderId);
        order.CreationTime.ShouldBeLessThanOrEqualTo(FakeClock.Now);
        order.LastModifierId.ShouldBe(Guid.Empty);
        await OrderRepository.DeleteAsync(order);

        order.IsDeleted.ShouldBe(true);
        order.LastModifierId.ShouldBe(CurrentUserId);
        Debug.Assert(order.LastModificationTime != null, "order.LastModificationTime != null");
        order.LastModificationTime.Value.ShouldBeLessThanOrEqualTo(FakeClock.Now);

        //TODO：被删除（软）的数据不应该被查询到（默认情况下）
        order = await OrderRepository.FirstOrDefaultAsync(x => x.Id == AppTestDataBuilder.OrderId);
        order.ShouldBeNull();
    }
}