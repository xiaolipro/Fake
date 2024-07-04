using Domain.Queries;
using Fake.AppTests.Tests;
using Fake.EntityFrameworkCore.Tests.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.EntityFrameworkCore.Tests.AppTests;

public class NonRootRepositoryTests : RootlessRepositoryTests<FakeEntityFrameworkCoreTestModule>
{
    protected override void BeforeAddFakeApplication(IServiceCollection services)
    {
        services.AddTransient<IOrderQueries, OrderQueries>();
    }
}