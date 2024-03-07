using Domain.Aggregates.BuyerAggregate;
using Domain.Aggregates.OrderAggregate;
using Domain.Repositories;
using Fake.DomainDrivenDesign.Repositories;
using Fake.DomainDrivenDesign.Repositories.EntityFrameWorkCore;
using Fake.EntityFrameworkCore;
using Fake.Helpers;
using Fake.Modularity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            typeof(OrderRepository));
        context.Services.AddTransient(typeof(IBuyerRepository),
            typeof(BuyerRepository));

        context.Services.AddDbContextFactory<OrderingContext>(builder =>
        {
            //使用sqlite内存模式要open
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            builder.UseSqlite(connection);
#if DEBUG
            builder.EnableSensitiveDataLogging();
            // var sp = context.Services.GetObjectAccessorOrNull<IServiceProvider>();
            // builder.AddInterceptors(sp.Value!.GetRequiredService<FakeDbCommandInterceptor>());
#endif
        });

        context.Services.Replace(new ServiceDescriptor(typeof(OrderingContext), typeof(OrderingContext),
            ServiceLifetime.Transient));
    }


    public override void PreConfigureApplication(ApplicationConfigureContext context)
    {
        var orderingContext = context.ServiceProvider.GetRequiredService<OrderingContext>();

        using (orderingContext)
        {
            AsyncHelper.RunSync(() => SeedAsync(orderingContext));
        }
    }


    private async Task SeedAsync(OrderingContext context)
    {
        await context.Database.EnsureCreatedAsync();
        await context.Database.MigrateAsync();

        await context.SaveChangesAsync();
    }
}