using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Fake.Threading;

namespace Fake.Identity.Security.Claims;

public interface IFakeClaimsPrincipalFactory
{
    Task<ClaimsPrincipal> CreateAsync(ClaimsPrincipal claimsPrincipal = null);
}