using Fake.DependencyInjection;
using Tests;

namespace Fake.EntityFrameworkCore.Tests.AppTests;

public class UowTests : UowTests<FakeEntityFrameworkCoreTestModule>, ITransientDependency
{
}