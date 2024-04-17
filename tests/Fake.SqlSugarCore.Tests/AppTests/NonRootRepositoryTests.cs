using Domain.Queries;
using Fake.SqlSugarCore.Tests.Queries;
using Microsoft.Extensions.DependencyInjection;
using Tests;

namespace Fake.SqlSugarCore.Tests.AppTests;

public class NonRootRepositoryTests : RootlessRepositoryTests<FakeSqlSugarCoreTestModule>
{
    protected override void BeforeAddFakeApplication(IServiceCollection services)
    {
        services.AddTransient<IOrderQueries, OrderQueries>();
    }
}