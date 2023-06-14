using Fake.Modularity;

namespace Fake.Http.Tests;

[DependsOn(typeof(FakeHttpModule))]
public class FakeHttpTestModule : FakeModule
{

}