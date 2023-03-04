


using Fake.Autofac;
using Fake.Modularity;

[DependsOn(typeof(FakeAutofacModule),typeof(FakeCoreTestModule))]
public class FakeAutofacTestModule:FakeModule
{
}