using Fake.Auditing;
using Fake.EntityFrameworkCore.Auditing;
using Fake.EntityFrameworkCore.Interceptors;
using Fake.Modularity;
using Fake.UnitOfWork;
using Fake.UnitOfWork.EntityFrameWorkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


// ReSharper disable once CheckNamespace
namespace Fake.EntityFrameworkCore;

[DependsOn(typeof(FakeAuditingModule), typeof(FakeUnitOfWorkModule))]
public class FakeEntityFrameworkCoreModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
        context.Services.AddTransient<IEntityChangeHelper, EntityChangeHelper>();
        context.Services.AddSingleton<FakeDbCommandInterceptor>();
        context.Services.AddSingleton<ICommandFormatter, FakeCommandFormatter>();
    }
}