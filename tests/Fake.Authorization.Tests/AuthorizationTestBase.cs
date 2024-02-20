using System.Security.Claims;
using Fake.Authorization.Tests;
using Fake.IdGenerators.GuidGenerator;
using Fake.Security.Claims;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

public class AuthorizationTestBase : ApplicationTest<FakeAuthorizationTestModule>
{
    protected override void AfterAddFakeApplication(IServiceCollection services)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, "FakeName"),
            new(ClaimTypes.NameIdentifier, SimpleGuidGenerator.Instance.GenerateAsString()),
            new(ClaimTypes.Role, "FakeRole")
        };

        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var principalAccessor = Substitute.For<ICurrentPrincipalAccessor>();
        principalAccessor.Principal.Returns(_ => claimsPrincipal);
        Thread.CurrentPrincipal = claimsPrincipal;
    }
}