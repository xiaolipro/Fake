using Fake.Modularity;
using Fake.Testing;

[DependsOn(typeof(FakeUnitOfWorkModule))]
[DependsOn(typeof(FakeTestingModule))]
public class FakeUnitOfWorkTestModule:FakeModule
{
    
}