using Fake.Auditing;
using Fake.Autofac;
using Fake.Modularity;

[DependsOn(typeof(FakeAuditingModule), typeof(FakeAutofacModule))]
public class FakeAuditingTestModule : FakeModule
{
}