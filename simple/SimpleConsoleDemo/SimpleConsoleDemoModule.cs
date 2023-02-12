using Fake.Auditing;
using Fake.Autofac;
using Fake.Identity;
using Fake.Modularity;

namespace SimpleConsoleDemo;

[DependsOn(typeof(FakeAuditingModule),typeof(FakeAutofacModule),typeof(FakeIdentityModule))]
public class SimpleConsoleDemoModule:FakeModule
{
    
}