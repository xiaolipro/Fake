using Domain.Queries;
using Microsoft.Extensions.DependencyInjection;
using Tests;

namespace AppTests;

public class NonRootRepositoryTests : RootlessRepositoryTests<FakeEntityFrameworkCoreTestModule>
{
    protected override void BeforeAddFakeApplication(IServiceCollection services)
    {
        services.AddTransient<IOrderQueries, OrderQueries>();
    }
}