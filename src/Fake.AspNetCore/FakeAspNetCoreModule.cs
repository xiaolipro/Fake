﻿using Fake.AspNetCore.Auditing;
using Fake.AspNetCore.ExceptionHandling;
using Fake.AspNetCore.Http;
using Fake.AspNetCore.Security.Claims;
using Fake.Auditing;
using Fake.Identity;
using Fake.Identity.Security.Claims;
using Fake.Modularity;
using Fake.UnitOfWork;
using Fake.VirtualFileSystem;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Fake.AspNetCore;

[DependsOn(
    typeof(FakeIdentityModule),
    typeof(FakeAuditingModule),
    typeof(FakeUnitOfWorkModule),
    typeof(FakeVirtualFileSystemModule))]
public class FakeAspNetCoreModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpContextAccessor();
        context.Services.AddSingleton<ICurrentPrincipalAccessor, HttpContextCurrentPrincipalAccessor>();
        
        context.Services.AddObjectAccessor<IApplicationBuilder>();

        context.Services.AddTransient<IAuthorizationExceptionHandler, DefaultAuthorizationExceptionHandler>();
        context.Services.AddTransient<IHttpClientInfoProvider, HttpClientInfoProvider>();
        
        context.Services.Configure<FakeAuditingOptions>(options =>
        {
            options.Contributors.Add(new AspNetCoreAuditLogContributor());
        });
        
        context.Services.AddAuthorization();
    }
}