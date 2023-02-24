using Fake.Auditing;
using Fake.Autofac;
using Fake.Modularity;

[DependsOn(typeof(FakeAuditingModuleApplication), typeof(FakeAutofacModuleApplication))]
public class FakeAuditingTestModuleApplication : FakeModuleApplication
{
}