using System.Collections.Generic;
using System.Reflection;
using Fake.Castle.DynamicProxy;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.Castle;

public class FakeCastleModule : FakeModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(registrationContext =>
        {
            foreach (var intercept in
                     registrationContext.ImplementationType.GetCustomAttributes<FakeInterceptAttribute>(true))
            {
                registrationContext.Interceptors.TryAdd(intercept.InterceptorType);
            }
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient(typeof(FakeAsyncDeterminationInterceptor<>));
    }
}