using Fake.Modularity;

namespace Fake.Auditing;

[DependsOn(typeof(FakeAuditingContractsModule))]
public class FakeAuditingModule:FakeModule
{
}