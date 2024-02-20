using System;
using System.Linq;
using Fake.Authorization.Localization;
using Fake.Authorization.Permissions;
using Fake.Authorization.Permissions.Contributors;
using Fake.DynamicProxy;
using Fake.Localization;

// ReSharper disable once CheckNamespace
namespace Fake.Authorization;

[DependsOn(typeof(FakeLocalizationModule))]
[DependsOn(typeof(FakeSecurityModule))]
public class FakeAuthorizationModule : FakeModule
{
    public override bool SkipServiceRegistration => true;

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(registrationContext =>
        {
            // 这里只是较粗粒度的判断，需不需要为类型添加拦截器，并非最终的拦截
            if (ShouldIntercept(registrationContext.ImplementationType))
            {
                registrationContext.Interceptors.TryAdd<AuthorizationInterceptor>();
            }
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAuthorizationCore();

        context.Services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandle>();
        context.Services.AddTransient<IAuthorizationPolicyProvider, FakeAuthorizationPolicyProvider>();

        context.Services.AddFakeVirtualFileSystem<FakeAuthorizationModule>("/Fake/Authorization");

        context.Services.Configure<FakeLocalizationOptions>(options =>
        {
            options.Resources
                .Add<FakeAuthorizationResource>("zh")
                .LoadVirtualJson("/Localization");
        });

        context.Services.AddTransient<AuthorizationInterceptor>();
        context.Services.AddSingleton<IPermissionManager, PermissionManager>();
        context.Services.AddSingleton<IPermissionStore, NullPermissionStore>();
        context.Services.AddTransient<IPermissionChecker, PermissionChecker>();
        context.Services.AddTransient<IPermissionCheckContributor, RolePermissionCheckContributor>();
        context.Services.AddTransient<IPermissionCheckContributor, UserPermissionCheckContributor>();
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