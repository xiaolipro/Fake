using System.Threading.Tasks;

namespace Fake.Identity.Security.Claims;

public interface IFakeClaimsPrincipalContributor
{
    Task ContributeAsync(FakeClaimsPrincipalContributorContext context);
}