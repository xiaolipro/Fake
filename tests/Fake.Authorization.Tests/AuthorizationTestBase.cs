using System.Security.Claims;
using Fake.Authorization.Tests;
using Fake.Identity.Security.Claims;
using Fake.IdGenerators.GuidGenerator;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

public class AuthorizationTestBase : FakeApplicationTest<FakeAuthorizationTestModule>
{
    protected override void AfterAddFakeApplication(IServiceCollection services)
    {
        var claims = new List<Claim>()
        {
            new Claim(FakeClaimTypes.UserName, "FakeName"),
            new Claim(FakeClaimTypes.UserId, SimpleGuidGenerator.Instance.GenerateAsString()),
            new Claim(FakeClaimTypes.Role, "FakeRole")
        };

        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var principalAccessor = Substitute.For<ICurrentPrincipalAccessor>();
        principalAccessor.Principal.Returns(ci => claimsPrincipal);
        Thread.CurrentPrincipal = claimsPrincipal;
    }
}