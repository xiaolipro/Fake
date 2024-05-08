using Fake.AspNetCore.Controller.Conventions;
using Fake.Modularity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Fake.AspNetCore.Controller;

[DependsOn(typeof(FakeAspNetCoreModule))]
public class FakeAspNetCoreControllerModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddControllers();
        context.Services.AddOptions<MvcOptions>()
            .Configure<IServiceProvider>((options, provider) =>
            {
                options.Conventions.Add(provider.GetRequiredService<ApplicationServiceConvention>());
            });
        context.Services.AddTransient<IApplicationServiceActionConventional, ApplicationServiceActionConventional>();

        //Add feature providers
        var partManager = context.Services.GetInstance<ApplicationPartManager>();
        var application = context.Services.GetInstance<IFakeApplication>();
        partManager.FeatureProviders.Add(new ApplicationServiceControllerFeatureProvider(application));
    }

    public override void ConfigureApplication(ApplicationConfigureContext context)
    {
        // in AddMvcCore
        var partManager = context.ServiceProvider.GetService<ApplicationPartManager>();
        if (partManager == null)
        {
            return;
        }

        var conventionalControllerAssemblies = context.ServiceProvider
            .GetRequiredService<IOptions<ApplicationServiceConventionOptions>>()
            .Value.Assemblies;

        foreach (var assembly in conventionalControllerAssemblies)
        {
            if (partManager.ApplicationParts.Any(p =>
                    p is AssemblyPart assemblyPart && assemblyPart.Assembly == assembly)) continue;
            partManager.ApplicationParts.Add(new AssemblyPart(assembly));
        }
    }
}