using System;
using System.Collections.Generic;
using Fake.Identity.Security.Claims;
using Fake.Identity.Users;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.Identity;

public class FakeIdentityModule : FakeModule
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