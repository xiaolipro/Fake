


using Fake.Autofac;
using Fake.Modularity;

[DependsOn(typeof(FakeAutofacModule))]
public class FakeAutofacTestModule:FakeModule
{
}