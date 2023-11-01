using System;
using System.Linq;
using Fake.Authorization;
using Fake.Authorization.Localization;
using Fake.DynamicProxy;
using Fake.Identity;
using Fake.Localization;
using Microsoft.AspNetCore.Authorization;

[DependsOn(typeof(FakeLocalizationModule))]
[DependsOn(typeof(FakeIdentityModule))]
public class FakeAuthorizationModule : FakeModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(registrationContext =>
        {
            // 这里只是较粗粒度的判断，需不需要为类型生成拦截器，并非最终的拦截
            if (ShouldIntercept(registrationContext.ImplementationType))
            {
                registrationContext.Interceptors.TryAdd<AuthorizationInterceptor>();
            }
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAuthorizationCore();

        context.Services.AddFakeVirtualFileSystem<FakeAuthorizationModule>("/Fake/Authorization");

        context.Services.Configure<FakeLocalizationOptions>(options =>
        {
            options.Resources
                .Add<FakeAuthorizationResource>("zh")
                .LoadVirtualJson("/Localization");
        });

        context.Services.AddTransient<AuthorizationInterceptor>();
        context.Services.AddTransient<IMethodAuthorizationService, MethodAuthorizationService>();
    }

    private static bool ShouldIntercept(Type type)
    {
        if (DynamicProxyIgnoreTypes.Contains(type)) return false;

        if (type.IsDefined(typeof(AllowAnonymousAttribute), true)) return false;
        if (type.IsDefined(typeof(AuthorizeAttribute), true)) return true;

        if (type.GetMethods().Any(m => m.IsDefined(typeof(AuthorizeAttribute), true))) return true;

        return false;
    }
}