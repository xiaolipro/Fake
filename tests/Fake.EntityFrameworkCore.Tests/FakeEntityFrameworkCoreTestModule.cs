using AppTests;
using AppTests.Repositories;
using Domain.Aggregates.OrderAggregate;
using Fake.EntityFrameworkCore;
using Fake.Modularity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[DependsOn(typeof(FakeAppTestModule))]
[DependsOn(typeof(FakeEntityFrameworkCoreModule))]
public class FakeEntityFrameworkCoreTestModule:FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IOrderRepository, OrderRepository>();

        context.Services.AddDbContextFactory<OrderingContext>(builder =>
        {
            /*builder.UseSqlite(new SqliteConnection("Data Source=:memory:"));*/
            builder.UseSqlite("FileName=./fake.db");
        });
    }
}