
using System;
using System.Collections.Generic;
using Fake.Identity.Security.Claims;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.Identity;

public class FakeIdentityModuleApplication:FakeModuleApplication
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
}