using Fake.Modularity;
using Fake.ObjectMapping;
using Microsoft.Extensions.DependencyInjection;

public class FakeObjectMappingModule:FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IAutoMappingProvider, NotImplementedAutoMappingProvider>();
    }
}