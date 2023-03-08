using Fake.AspNetCore;
using Fake.Autofac;
using Fake.Modularity;
using Fake.Testing;

[DependsOn(typeof(FakeAspNetCoreModule),
    typeof(FakeAutofacModule),
    typeof(FakeTestingModule))]
public class FakeAspNetCoreTestModule:FakeModule
{
    
}