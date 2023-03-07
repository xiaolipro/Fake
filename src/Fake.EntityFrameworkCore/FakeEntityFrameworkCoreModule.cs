using Fake.Auditing;
using Fake.Modularity;


// ReSharper disable once CheckNamespace
namespace Fake.EntityFrameworkCore;

[DependsOn(typeof(FakeAuditingModule), typeof(FakeUnitOfWorkModule))]
public class FakeEntityFrameworkCoreModule : FakeModule
{
}