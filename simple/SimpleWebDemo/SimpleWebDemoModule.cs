using Fake.AspNetCore;
using Fake.AspNetCore.Auditing;
using Fake.AspNetCore.Mvc.Conventions;
using Fake.Autofac;
using Fake.Modularity;

[DependsOn(typeof(FakeAutofacModule))]
[DependsOn(typeof(FakeAspNetCoreModule))]
public class SimpleWebDemoModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<RemoteServiceConventionOptions>(options =>
        {
            options.AddAssembly(typeof(SimpleWebDemoModule).Assembly);
        });
        context.Services.AddFakeSwaggerGen();
        context.Services.AddFakeAspNetCoreAuditing();
        context.Services.AddFakeExceptionFilter().AddFakeValidationActionFilter();
    }

    public override void ConfigureApplication(ApplicationConfigureContext context)
    {
        var app = context.GetWebApplication();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseFakeSwagger();

        app.MapControllers();
    }
}