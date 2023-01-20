using Fake.Auditing;
using Fake.Modularity;

[DependsOn(typeof(FakeAuditingModule))]
public class FakeAuditingTestModule:FakeModule
{
    
}