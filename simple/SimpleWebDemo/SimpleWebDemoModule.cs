﻿using Fake.AspNetCore;
using Fake.Auditing;
using Fake.Autofac;
using Fake.Modularity;

[DependsOn(typeof(FakeAuditingModule), typeof(FakeAutofacModule))]
[DependsOn(typeof(FakeAspNetCoreModule))]
public class SimpleWebDemoModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<FakeAuditingOptions>(options =>
        {
            // options.IsEnabledActionLog = false;
            // options.IsEnabledExceptionLog = false;
        });
    }

    public override void ConfigureApplication(ApplicationConfigureContext context)
    {
        var app = context.GetWebApplication();
        app.MapGet("/s", () => "Hello World!");
    }
}