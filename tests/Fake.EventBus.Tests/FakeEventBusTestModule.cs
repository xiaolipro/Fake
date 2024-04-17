using Fake.Modularity;
using Fake.Testing;

namespace Fake.EventBus.Tests;

[DependsOn(typeof(FakeTestingModule))]
[DependsOn(typeof(FakeEventBusModule))]
public class FakeEventBusTestModule : FakeModule
{
}