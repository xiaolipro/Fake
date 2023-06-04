using Fake.Modularity;
using Fake.Timing;

namespace Fake.Testing;

public abstract class FakeBasicIntegrationTest<TStartupModule>:FakeIntegrationTest<TStartupModule>
    where TStartupModule : IFakeModule
{
    protected IFakeClock FakeClock { get; }

    public FakeBasicIntegrationTest()
    {
        FakeClock = GetRequiredService<IFakeClock>();
    }
}