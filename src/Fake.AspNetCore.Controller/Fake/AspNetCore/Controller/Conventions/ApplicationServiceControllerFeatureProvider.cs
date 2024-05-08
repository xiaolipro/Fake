using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;

namespace Fake.AspNetCore.Controller.Conventions;

public class ApplicationServiceControllerFeatureProvider(IFakeApplication application) : ControllerFeatureProvider
{
    protected override bool IsController(TypeInfo typeInfo)
    {
        return application.ServiceProvider
            .GetRequiredService<IOptions<ApplicationServiceConventionOptions>>().Value
            .ControllerTypes.Contains(typeInfo.AsType());
    }
}