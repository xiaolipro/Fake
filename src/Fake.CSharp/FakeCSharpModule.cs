using Fake.Modularity;

namespace Fake.CSharp;

public class FakeCSharpModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);
    }

    public override void ConfigureApplication(ApplicationConfigureContext context)
    {
        base.ConfigureApplication(context);
    }
}
