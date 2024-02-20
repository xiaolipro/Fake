using System;
using System.Collections.Generic;
using Fake.Modularity;
using Fake.Security.Claims;
using Fake.Users;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake;

public class FakeSecurityModule : FakeModule
{
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        var contributorTypes = new List<Type>();

        context.Services.OnRegistered(registrationContext =>
        {
            if (registrationContext.ImplementationType.IsAssignableTo<IFakeClaimsPrincipalContributor>())
            {
                contributorTypes.Add(registrationContext.ImplementationType);
            }
        });

        context.Services.Configure<FakeClaimsPrincipalFactoryOptions>(options =>
        {
            foreach (var type in contributorTypes)
            {
                options.Contributors.TryAdd(type);
            }
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<ICurrentPrincipalAccessor, ThreadCurrentPrincipalAccessor>();

        context.Services.AddTransient<ICurrentUser, CurrentUser>();
    }
}