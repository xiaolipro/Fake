using Fake.Autofac;
using Fake.Modularity;

// ReSharper disable once CheckNamespace
namespace Fake.Testing;

[DependsOn(typeof(FakeAutofacModule))]
public class FakeTestingModule : FakeModule
{
}