using Domain.Queries;
using Fake.EntityFrameworkCore.Tests.Queries;
using Microsoft.Extensions.DependencyInjection;
using Tests;

namespace Fake.EntityFrameworkCore.Tests.AppTests;

public class NonRootRepositoryTests : RootlessRepositoryTests<FakeEntityFrameworkCoreTestModule>
{
    protected override void BeforeAddFakeApplication(IServiceCollection services)
    {
        services.AddTransient<IOrderQueries, OrderQueries>();
    }
}