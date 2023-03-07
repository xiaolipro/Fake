using Fake.Auditing;
using Fake.EntityFrameworkCore.UnitOfWork;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection.Extensions;


// ReSharper disable once CheckNamespace
namespace Fake.EntityFrameworkCore;

[DependsOn(typeof(FakeAuditingModule), typeof(FakeUnitOfWorkModule))]
public class FakeEntityFrameworkCoreModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddTransient(typeof(IEfCoreDbContextProvider<>), typeof(EfCoreUnitOfWorkDbContextProvider<>));
    }
}