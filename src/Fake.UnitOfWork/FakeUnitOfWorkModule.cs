using System;
using System.Reflection;
using Fake.DynamicProxy;
using Fake.Modularity;
using Fake.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

public class FakeUnitOfWorkModule:FakeModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(registrationContext =>
        {
            if (ShouldIntercept(registrationContext.ImplementationType))
            {
                registrationContext.Interceptors.TryAdd<UnitOfWorkInterceptor>();
            }
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IUnitOfWorkHelper, UnitOfWorkHelper>();
        context.Services.AddSingleton<IUnitOfWorkTransactionalProvider, NullUnitOfWorkTransactionalProvider>();
        context.Services.AddSingleton<IUnitOfWorkManager, UnitOfWorkManager>();
    }

    private static bool ShouldIntercept(Type type)
    {
        if (DynamicProxyIgnoreTypes.Contains(type)) return false;
        return UnitOfWorkHelper.IsUnitOfWorkType(type.GetTypeInfo());
    }
}