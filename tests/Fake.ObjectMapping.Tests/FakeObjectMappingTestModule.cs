using Fake.Modularity;
using Fake.Testing;

[DependsOn(typeof(FakeObjectMappingModule))]
[DependsOn(typeof(FakeTestingModule))]
public class FakeObjectMappingTestModule : FakeModule
{
}