using System;
using System.Collections.Generic;
using System.Linq;
using Fake.DependencyInjection;
using Fake.Proxy;

namespace Fake.Auditing;

public static class AuditingInterceptorRegistrar
{
    public static void RegisterIfNeeded(OnServiceRegistrationContext context)
    {
        // 这里只是较粗粒度的判断，需不需要为类型生成拦截器，并非最终的拦截
        if (ShouldIntercept(context.ImplementationType))
        {
            context.Interceptors.TryAdd(typeof(AuditingInterceptor));
        }
    }

    private static bool ShouldIntercept(Type type)
    {
        if (DynamicProxyIgnoreTypes.Contains(type)) return false;

        //TODO：在继承链中，最好先检查顶层类的attributes
        if (type.IsDefined(typeof(AuditedAttribute), true)) return true;
        if (type.IsDefined(typeof(DisableAuditingAttribute), true)) return false;

        if (type.GetMethods().Any(m => m.IsDefined(typeof(AuditedAttribute), true))) return true;

        return false;
    }
}