using Fake.Auditing;
using Fake.Autofac;
using Fake.Modularity;

namespace SimpleConsoleDemo;

[DependsOn(typeof(FakeAuditingModule),typeof(FakeAutofacModule))]
public class SimpleConsoleDemoModule:FakeModule
{
    
}