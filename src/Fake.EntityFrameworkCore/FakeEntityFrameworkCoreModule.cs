using Fake.EntityFrameworkCore.Auditing;
using Fake.EntityFrameworkCore.Interceptors;
using Fake.Modularity;
using Fake.UnitOfWork.EntityFrameWorkCore;
using Microsoft.Extensions.DependencyInjection;


// ReSharper disable once CheckNamespace
namespace Fake.EntityFrameworkCore;

[DependsOn(typeof(FakeDomainDrivenDesignModule))]
public class FakeEntityFrameworkCoreModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient(typeof(IEfDbContextProvider<>), typeof(UowEfDbContextProvider<>));
        context.Services.AddTransient<IEntityChangeHelper, EntityChangeHelper>();
        context.Services.AddSingleton<FakeDbCommandInterceptor>();
        context.Services.AddSingleton<ICommandFormatter, FakeCommandFormatter>();
    }
}