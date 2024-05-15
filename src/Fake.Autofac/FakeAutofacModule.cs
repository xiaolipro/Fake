using Fake.Castle;
using Fake.Modularity;

// ReSharper disable once CheckNamespace
namespace Fake.Autofac;

[DependsOn(typeof(FakeCastleModule))]
public class FakeAutofacModule : FakeModule;