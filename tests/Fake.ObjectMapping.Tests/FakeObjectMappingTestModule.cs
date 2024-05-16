using Fake.Modularity;
using Fake.Testing;

namespace Fake.ObjectMapping.Tests;

[DependsOn(typeof(FakeObjectMappingModule))]
[DependsOn(typeof(FakeTestingModule))]
public class FakeObjectMappingTestModule : FakeModule
{
}