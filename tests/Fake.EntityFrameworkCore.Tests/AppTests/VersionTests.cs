using Fake.DependencyInjection;
using Tests;

namespace AppTests;

public class VersionTests : VersionTests<FakeEntityFrameworkCoreTestModule>,ITransientDependency
{
}