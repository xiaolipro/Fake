using Fake.Modularity;
using Fake.Timing;

namespace Fake.Testing;

public abstract class FakeApplicationTestWithTools<TStartupModule> : FakeApplicationTest<TStartupModule>
    where TStartupModule : IFakeModule
{
    protected IFakeClock FakeClock { get; }

    public FakeApplicationTestWithTools()
    {
        FakeClock = GetRequiredService<IFakeClock>();
    }
}