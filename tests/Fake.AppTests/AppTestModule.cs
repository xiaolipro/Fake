
using Fake.Autofac;
using Fake.Modularity;

[DependsOn(typeof(FakeAutofacModule))]
[DependsOn(typeof(FakeDddDomainModule))]
public class FakeAppTestModule:FakeModule
{
}