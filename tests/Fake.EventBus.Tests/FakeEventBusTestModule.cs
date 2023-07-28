

using Fake.EventBus;
using Fake.Modularity;
using Fake.Testing;

[DependsOn(typeof(FakeTestingModule))]
[DependsOn(typeof(FakeEventBusModule))]
public class FakeEventBusTestModule:FakeModule
{
    
}