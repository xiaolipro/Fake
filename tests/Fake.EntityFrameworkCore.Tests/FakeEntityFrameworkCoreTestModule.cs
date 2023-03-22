using AppTests;
using AppTests.Repositories;
using Domain.Aggregates.OrderAggregate;
using Fake.EntityFrameworkCore;
using Fake.Modularity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

[DependsOn(typeof(FakeAppTestModule))]
[DependsOn(typeof(FakeEntityFrameworkCoreModule))]
public class FakeEntityFrameworkCoreTestModule:FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IOrderRepository, OrderRepository>();

        context.Services.AddDbContextFactory<OrderingContext>(builder =>
        {
            builder.UseSqlite(new SqliteConnection("Data Source=:memory:"));
            /*builder.UseSqlite("FileName=./fake.db");*/
        });

    }

    public override void PreConfigureApplication(ApplicationConfigureContext context)
    {
        var ctx = context.ServiceProvider.GetService<OrderingContext>();
        ctx.Database.EnsureDeleted();
        ctx.Database.EnsureCreated();
    }

    public override void PreConfigureApplication(ApplicationConfigureContext context)
    {
        var orderingContext = context.ServiceProvider.GetService<OrderingContext>();
        orderingContext.Database.EnsureCreated();
    }
}