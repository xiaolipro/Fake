using System;
using System.Reflection;
using Fake.DynamicProxy;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.UnitOfWork;

public class FakeUnitOfWorkModule : FakeModule
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
        context.Services.AddSingleton<IAmbientUnitOfWorkProvider, AmbientUnitOfWorkProvider>();
        context.Services.AddSingleton<IUnitOfWorkManager, UnitOfWorkManager>();

        context.Services.AddTransient<IUnitOfWork, UnitOfWork>();
        context.Services.AddTransient<UnitOfWorkInterceptor>();

        // 工作单元全局默认配置，优先级别次于UnitOfWorkAttribute
        context.Services.Configure<FakeUnitOfWorkOptions>(_ => { });
    }

    private static bool ShouldIntercept(Type type)
    {
        if (DynamicProxyIgnoreTypes.Contains(type)) return false;
        return UnitOfWorkHelper.IsUnitOfWorkType(type.GetTypeInfo());
    }
}