using Fake.DependencyInjection;
using Tests;

namespace Fake.EntityFrameworkCore.Tests.AppTests;

public class VersionTests : VersionTests<FakeEntityFrameworkCoreTestModule>, ITransientDependency
{
}