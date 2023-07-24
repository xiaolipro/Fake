using Fake.Auditing;
using Fake.EntityFrameworkCore.Interceptors;
using Fake.Modularity;
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
        
        context.Services.AddSingleton<FakeDbCommandInterceptor>();
        context.Services.AddSingleton<ICommandFormatter, FakeCommandFormatter>();
    }
}