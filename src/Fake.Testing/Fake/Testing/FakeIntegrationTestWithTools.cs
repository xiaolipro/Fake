using Fake.Modularity;
using Fake.Timing;

namespace Fake.Testing;

public abstract class FakeIntegrationTestWithTools<TStartupModule> : FakeIntegrationTest<TStartupModule>
    where TStartupModule : IFakeModule
{
    protected IFakeClock FakeClock { get; }

    public FakeIntegrationTestWithTools()
    {
        FakeClock = GetRequiredService<IFakeClock>();
    }
}