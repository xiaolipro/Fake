using Fake.Modularity;

namespace Fake.Http.Tests;

[DependsOn(typeof(FakeAppTestModule))]
[DependsOn(typeof(FakeHttpModule))]
public class FakeHttpTestModule : FakeModule
{
    
}