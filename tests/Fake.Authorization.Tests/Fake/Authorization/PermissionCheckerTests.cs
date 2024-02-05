using Fake.Authorization.Permissions;
using Shouldly;
using Xunit;

namespace Fake.Authorization;

public class PermissionCheckerTests : AuthorizationTestBase
{
    private readonly IPermissionChecker _permissionChecker;

    public PermissionCheckerTests()
    {
        _permissionChecker = GetRequiredService<IPermissionChecker>();
    }

    [Fact]
    public async Task IsGrantedAsync()
    {
        (await _permissionChecker.IsGrantedAsync("user", "user.create")).ShouldBe(true);
        (await _permissionChecker.IsGrantedAsync("system")).ShouldBe(false);
    }
}