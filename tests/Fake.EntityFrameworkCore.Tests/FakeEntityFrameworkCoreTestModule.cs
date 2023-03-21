using Domain.Aggregates.OrderAggregate;
using Fake.EntityFrameworkCore.Tests.AppTests.Repositories;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.EntityFrameworkCore.Tests;

[DependsOn(typeof(FakeAppTestModule))]
[DependsOn(typeof(FakeEntityFrameworkCoreModule))]
public class FakeEntityFrameworkCoreTestModule:FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IOrderRepository, OrderRepository>();
    }
}