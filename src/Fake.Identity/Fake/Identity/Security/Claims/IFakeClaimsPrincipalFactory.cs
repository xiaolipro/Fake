using System.Security.Claims;
using System.Threading.Tasks;

namespace Fake.Identity.Security.Claims;

public interface IFakeClaimsPrincipalFactory
{
    Task<ClaimsPrincipal?> CreateAsync(ClaimsPrincipal? claimsPrincipal = null);
}