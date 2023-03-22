using AppTests;
using AppTests.Repositories;
using Domain.Aggregates.OrderAggregate;
using Fake.EntityFrameworkCore;
using Fake.Modularity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
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
            //使用sqlite内存模式要开open
            var options = new SqliteConnection("Filename=:memory:");
            options.Open();
            builder.UseSqlite(options);
            /*builder.UseSqlite("FileName=./fake.db");*/
        });

    }

    public override void PreConfigureApplication(ApplicationConfigureContext context)
    {
        var ctx = context.ServiceProvider.GetService<OrderingContext>();
        
        ctx.Database.EnsureDeleted();
        ctx.Database.EnsureCreated();
    }

}