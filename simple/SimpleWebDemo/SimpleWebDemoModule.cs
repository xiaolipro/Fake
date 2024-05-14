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
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<FakeAuditingOptions>(options =>
        {
            // options.IsEnabledActionLog = false;
            // options.IsEnabledExceptionLog = false;
        });

        context.Services.Configure<RemoteServiceConventionOptions>(options =>
        {
            options.AddAssembly(typeof(SimpleWebDemoModule).Assembly);
        });

        context.Services.AddEndpointsApiExplorer();
        context.Services.AddSwaggerGen();
    }

    public override void ConfigureApplication(ApplicationConfigureContext context)
    {
        var app = context.GetWebApplication();
        app.MapGet("/s", () => "Hello World!");
        app.MapGet("/b", () => "Hello World!");

        app.UseSwagger();
        app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Simple Api"); });
    }
}