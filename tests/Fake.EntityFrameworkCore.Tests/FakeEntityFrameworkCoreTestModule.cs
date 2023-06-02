﻿using Domain.Aggregates.BuyerAggregate;
using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Repositories;
using Fake.Domain.Repositories.EntityFrameWorkCore;
using Fake.EntityFrameworkCore;
using Fake.Modularity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repositories;

[DependsOn(typeof(FakeAppTestModule))]
[DependsOn(typeof(FakeEntityFrameworkCoreModule))]
public class FakeEntityFrameworkCoreTestModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient(typeof(IRepository<Order>),
            typeof(EfCoreRepository<OrderingContext, Order>));
        context.Services.AddTransient(typeof(IRepository<Buyer>),
            typeof(EfCoreRepository<OrderingContext, Buyer>));
        context.Services.AddTransient(typeof(IOrderRepository),
            typeof(OrderRepository));
        context.Services.AddDbContextFactory<OrderingContext>(builder =>
        {
            //使用sqlite内存模式要开open
            var options = new SqliteConnection("Filename=:memory:");
            options.Open();
            builder.UseSqlite(options).UseLoggerFactory(LoggerFactory.Create(loggingBuilder =>
            {
                loggingBuilder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information) // 仅记录命令信息
                    .AddConsole(); // 输出到控制台
            }));
#if DEBUG
            builder.EnableSensitiveDataLogging();
#endif
            /*builder.UseSqlite("FileName=./fake.db");*/
        });
    }

    public override void PreConfigureApplication(ApplicationConfigureContext context)
    {
        var ctx = context.ServiceProvider.GetRequiredService<OrderingContext>();

        ctx.Database.EnsureCreated();
    }
}