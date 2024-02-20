using System.Security.Claims;
using Fake.Authorization.Services;
using Fake.IdGenerators.GuidGenerator;
using Fake.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Fake.Authorization;

public class AuthorizationTests : AuthorizationTestBase
{
    private readonly SimpleAService _simpleAService;
    private readonly SimpleBService _simpleBService;
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;
    private readonly UserService _userService;
    private readonly SystemService _systemService;

    public AuthorizationTests()
    {
        _userService = ServiceProvider.GetRequiredService<UserService>();
        _systemService = GetRequiredService<SystemService>();
        _simpleAService = ServiceProvider.GetRequiredService<SimpleAService>();
        _simpleBService = ServiceProvider.GetRequiredService<SimpleBService>();
        _currentPrincipalAccessor = ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
    }


    [Fact]
    async Task 未授权但访问授权方法会抛异常()
    {
        await Assert.ThrowsAsync<FakeAuthorizationException>(async () => { await _simpleAService.ProtectedAsync(); });

        await Assert.ThrowsAsync<FakeAuthorizationException>(async () =>
        {
            await _simpleBService.ProtectedByClassAsync();
        });
    }

    [Fact]
    async Task 未授权可以访问无须授权的方法()
    {
        var res = await _simpleAService.NormalAsync();
        res.ShouldBe(42);

        var res2 = await _simpleBService.AnonymousAsync();
        res2.ShouldBe(42);
    }

    [Fact]
    async Task 授权可以访问所有无policy的方法()
    {
        var claim = new Claim(ClaimTypes.NameIdentifier,
            SimpleGuidGenerator.Instance.GenerateAsString());
        using var _ = _currentPrincipalAccessor.Change(new ClaimsIdentity(claims: new[] { claim },
            authenticationType: "bearer"));
        await _simpleAService.ProtectedAsync();
        await _simpleBService.ProtectedByClassAsync();
    }

    [Fact]
    async Task 基于Policy的授权()
    {
        await _userService.CreateAsync();
        await _userService.DeleteAsync();

        await Assert.ThrowsAsync<FakeAuthorizationException>(async () => { await _systemService.DoSomethingAsync(); });
    }

    [Fact]
    async Task 基于Roles的授权()
    {
        await Assert.ThrowsAsync<FakeAuthorizationException>(async () =>
        {
            await _systemService.ProtectedByRoleAsync();
        });

        var claim = new Claim(ClaimTypes.Role, "admin");
        using var _ = _currentPrincipalAccessor.Change(claim);
        await _systemService.ProtectedByRoleAsync();
    }

    [Fact]
    async Task 基于Schemes的授权()
    {
        await Assert.ThrowsAsync<FakeAuthorizationException>(async () => { await _systemService.ProtectedByScheme(); });

        var claim = new Claim(ClaimTypes.Role, "admin");
        using var _ = _currentPrincipalAccessor.Change(new ClaimsIdentity(new List<Claim> { claim }, "QQ"));
        await _systemService.ProtectedByScheme();
    }
}