using Fake.DependencyInjection;
using Tests;

namespace AppTests;

public class UowTests : UowTests<FakeEntityFrameworkCoreTestModule>,ITransientDependency
{
}