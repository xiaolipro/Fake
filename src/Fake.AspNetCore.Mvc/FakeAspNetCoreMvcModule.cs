using Fake.AspNetCore.Mvc.ApiExplorer;
using Fake.AspNetCore.Mvc.Conventions;
using Fake.DomainDrivenDesign;
using Fake.Modularity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Fake.AspNetCore.Mvc;

[DependsOn(typeof(FakeAspNetCoreModule))]
[DependsOn(typeof(FakeDomainDrivenDesignModule))]
public class FakeAspNetCoreMvcModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //Use DI to create controllers
        context.Services.AddControllers();

        //Add feature providers
        var partManager = context.Services.GetInstance<ApplicationPartManager>();
        var application = context.Services.GetInstance<IFakeApplication>();
        partManager.FeatureProviders.Add(new RemoteServiceControllerFeatureProvider(application));
        partManager.ApplicationParts.TryAdd(new AssemblyPart(typeof(FakeAspNetCoreModule).Assembly));

        context.Services.AddTransient<IActionDescriptorProvider, RemoteServiceActionDescriptorProvider>();
        context.Services.AddTransient<IApiDescriptionProvider, RemoteServiceApiDescriptionProvider>();
        context.Services.AddOptions<MvcOptions>()
            .Configure(options =>
            {
                var conventionOptions = context.Services.GetRequiredService<IOptions<RemoteServiceConventionOptions>>();
                var actionConventional = context.Services.GetRequiredService<IRemoteServiceActionHelper>();

                options.Conventions.Add(new RemoteServiceConvention(conventionOptions, actionConventional));
            });
        context.Services.AddTransient<IRemoteServiceActionHelper, RemoteServiceActionHelper>();
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
            .GetRequiredService<IOptions<RemoteServiceConventionOptions>>()
            .Value.Assemblies;

        foreach (var assembly in conventionalControllerAssemblies)
        {
            if (partManager.ApplicationParts.Any(p =>
                    p is AssemblyPart assemblyPart && assemblyPart.Assembly == assembly)) continue;
            partManager.ApplicationParts.Add(new AssemblyPart(assembly));
        }
    }
}