﻿using Domain.Aggregates.BuyerAggregate;
using Domain.Aggregates.OrderAggregate;
using Fake.Domain;
using Fake.Domain.Repositories;
using Fake.Domain.Repositories.EntityFrameWorkCore;
using Fake.EntityFrameworkCore;
using Fake.EntityFrameworkCore.Interceptors;
using Fake.Helpers;
using Fake.Modularity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Repositories;

[DependsOn(typeof(FakeAppTestModule))]
[DependsOn(typeof(FakeEntityFrameworkCoreModule))]
public class FakeEntityFrameworkCoreTestModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient(typeof(IRepository<Order>),
            typeof(EfCoreEfCoreRepository<OrderingContext, Order>));
        context.Services.AddTransient(typeof(IRepository<Buyer>),
            typeof(EfCoreEfCoreRepository<OrderingContext, Buyer>));
        context.Services.AddTransient(typeof(IOrderRepository),
            typeof(OrderEfCoreEfCoreRepository));
        context.Services.AddTransient(typeof(IBuyerRepository),
            typeof(BuyerEfCoreEfCoreRepository));

        context.Services.AddDbContextFactory<OrderingContext>(builder =>
        {
            //使用sqlite内存模式要open
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

           
            builder.UseSqlite(connection);


#if DEBUG
            builder.AddInterceptors(new FakeDbCommandInterceptor());
#endif
        });


        context.Services.Replace(new ServiceDescriptor(typeof(OrderingContext), typeof(OrderingContext),
            ServiceLifetime.Transient));
    }


    public override void PreConfigureApplication(ApplicationConfigureContext context)
    {
        var ctx = context.ServiceProvider.GetRequiredService<OrderingContext>();

        ctx.Database.EnsureCreated();

        AsyncHelper.RunSync(() => SeedAsync(ctx));
    }


    private async Task SeedAsync(OrderingContext context)
    {
        var cardTypes = Enumeration.GetAll<CardType>();
        context.CardTypes.AddRange(cardTypes);

        var orderStatus = Enumeration.GetAll<OrderStatus>();
        context.OrderStatus.AddRange(orderStatus);
        await context.SaveChangesAsync();
    }
}