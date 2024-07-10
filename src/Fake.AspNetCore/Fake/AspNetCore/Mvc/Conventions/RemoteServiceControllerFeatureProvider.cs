using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Fake.AspNetCore.Mvc.Conventions;

public class RemoteServiceControllerFeatureProvider(IFakeApplication application) : ControllerFeatureProvider
{
    protected override bool IsController(TypeInfo typeInfo)
    {
        return application.ServiceProvider
            .GetRequiredService<IOptions<ApplicationService2ControllerOptions>>().Value
            .ControllerTypes.Contains(typeInfo.AsType());
    }
}