using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fake.Identity.Security.Claims;

public interface IFakeClaimsPrincipalFactory
{
    Task<ClaimsPrincipal> CreateAsync(ClaimsPrincipal claimsPrincipal = null);
}

public class FakeClaimsPrincipalFactory : IFakeClaimsPrincipalFactory
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FakeClaimsPrincipalFactory(IServiceScopeFactory serviceScopeFactory, IOptions<FakeClaimsPrincipalFactoryOptions>)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    public Task<ClaimsPrincipal> CreateAsync(ClaimsPrincipal claimsPrincipal = null)
    {
        throw new System.NotImplementedException();
    }
}

public class FakeClaimsPrincipalFactoryOptions
{
    
}