using Fake.Autofac;
using Fake.Core.Tests;
using Fake.Modularity;

[DependsOn(typeof(FakeAutofacModule), typeof(FakeCoreTestModule))]
public class FakeAutofacTestModule : FakeModule
{
}