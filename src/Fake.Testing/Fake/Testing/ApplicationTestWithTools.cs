using Fake.Modularity;
using Fake.Timing;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Testing;

public abstract class ApplicationTestWithTools<TStartupModule> : ApplicationTest<TStartupModule>
    where TStartupModule : IFakeModule
{
    protected IFakeClock FakeClock { get; }

    public ApplicationTestWithTools()
    {
        FakeClock = ServiceProvider.GetRequiredService<IFakeClock>();
    }
}