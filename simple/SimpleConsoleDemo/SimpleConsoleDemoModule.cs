using Fake.Auditing;
using Fake.Modularity;

namespace SimpleConsoleDemo;

[DependsOn(typeof(FakeAuditingModule))]
public class SimpleConsoleDemoModule:FakeModule
{
    
}