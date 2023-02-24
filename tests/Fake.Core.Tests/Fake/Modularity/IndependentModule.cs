namespace Fake.Modularity;

public class IndependentModuleApplication:FakeModuleApplication
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        base.PreConfigureServices(context);
    }
}