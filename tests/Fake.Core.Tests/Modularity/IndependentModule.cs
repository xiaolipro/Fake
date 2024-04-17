using Fake.Modularity;

namespace Fake.Core.Tests.Modularity;

public class IndependentModule : FakeModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        base.PreConfigureServices(context);
    }
}