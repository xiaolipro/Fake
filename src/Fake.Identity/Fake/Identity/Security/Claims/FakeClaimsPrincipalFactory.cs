using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fake.Identity.Security.Claims;

public class FakeClaimsPrincipalFactory : IFakeClaimsPrincipalFactory
{
    public static string AuthenticationType = "Fake.Application";
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly FakeClaimsPrincipalFactoryOptions _fakeClaimsPrincipalOptions;

    public FakeClaimsPrincipalFactory(IServiceScopeFactory serviceScopeFactory, IOptions<FakeClaimsPrincipalFactoryOptions> fakeClaimsPrincipalOptions )
    {
        _serviceScopeFactory = serviceScopeFactory;
        _fakeClaimsPrincipalOptions = fakeClaimsPrincipalOptions.Value;
    }
    public async Task<ClaimsPrincipal> CreateAsync(ClaimsPrincipal claimsPrincipal = null)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        claimsPrincipal ??=
            new ClaimsPrincipal(new ClaimsIdentity(AuthenticationType, FakeClaimTypes.UserName, FakeClaimTypes.Role));

        var context = new FakeClaimsPrincipalContributorContext(claimsPrincipal, scope.ServiceProvider);

        foreach (var contributorType in _fakeClaimsPrincipalOptions.Contributors)
        {
            var contributor = scope.ServiceProvider.GetRequiredService(contributorType) as IFakeClaimsPrincipalContributor;

            Debug.Assert(contributor != null, nameof(contributor) + " != null");
            await contributor.ContributeAsync(context);
        }

        return claimsPrincipal;
    }
}