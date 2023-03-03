using Fake.Castle.DynamicProxy;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.Castle;
public class FakeCastleModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient(typeof(FakeAsyncDeterminationInterceptor<>));
    }
}