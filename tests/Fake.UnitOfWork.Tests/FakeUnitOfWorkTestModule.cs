using Fake.Modularity;
using Fake.Testing;

namespace Fake.UnitOfWork.Tests;

[DependsOn(typeof(FakeUnitOfWorkModule))]
[DependsOn(typeof(FakeTestingModule))]
public class FakeUnitOfWorkTestModule:FakeModule
{
    
}