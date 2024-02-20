using Fake.Autofac;
using Fake.Modularity;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Fake.AspNetCore.Testing;

[DependsOn(typeof(FakeAspNetCoreModule))]
[DependsOn(typeof(FakeTestingModule))]
[DependsOn(typeof(FakeAutofacModule))]
public class FakeAspNetCoreTestingModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddSingleton<ITestServerAccessor, TestServerAccessor>();
    }
}