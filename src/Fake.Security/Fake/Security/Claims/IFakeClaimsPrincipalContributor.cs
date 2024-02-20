using System.Threading.Tasks;

namespace Fake.Security.Claims;

public interface IFakeClaimsPrincipalContributor
{
    Task ContributeAsync(FakeClaimsPrincipalContributorContext context);
}