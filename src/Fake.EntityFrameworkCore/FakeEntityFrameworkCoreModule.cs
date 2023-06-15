using Fake.Auditing;
using Fake.Modularity;
using Fake.UnitOfWork.EntityFrameWorkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;


// ReSharper disable once CheckNamespace
namespace Fake.EntityFrameworkCore;

[DependsOn(typeof(FakeAuditingModule), typeof(FakeUnitOfWorkModule))]
public class FakeEntityFrameworkCoreModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddTransient(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
    }
}