using Fake.AspNetCore;
using Fake.AspNetCore.Mvc;
using Fake.AspNetCore.Mvc.Conventions;
using Fake.Auditing;
using Fake.Autofac;
using Fake.Modularity;

[DependsOn(typeof(FakeAuditingModule), typeof(FakeAutofacModule))]
[DependsOn(typeof(FakeAspNetCoreMvcModule))]
public class SimpleWebDemoModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<RemoteServiceConventionOptions>(options =>
        {
            options.AddAssembly(typeof(SimpleWebDemoModule).Assembly);
        });

        context.Services.AddUnifiedResultFilter();

        context.Services.AddFakeSwaggerGen();
    }

    public override void ConfigureApplication(ApplicationConfigureContext context)
    {
        var app = context.GetWebApplication();
        app.MapGet("/s", () => "Hello World!");
        app.MapGet("/b", () => "Hello World!");

        app.UseFakeSwagger();
    }
}