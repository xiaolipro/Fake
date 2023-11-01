using Fake.Modularity;
using Fake.Testing;

// ReSharper disable once CheckNamespace
namespace Fake.Authorization.Tests;

[DependsOn(typeof(FakeAuthorizationModule))]
[DependsOn(typeof(FakeTestingModule))]
public class FakeAuthorizationTestModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}