
using Fake.Castle;
using Fake.Modularity;

// ReSharper disable once CheckNamespace
namespace Fake.Autofac;

[DependsOn(typeof(FakeCastleModuleApplication))]
public class FakeAutofacModuleApplication:FakeModuleApplication
{
    
}