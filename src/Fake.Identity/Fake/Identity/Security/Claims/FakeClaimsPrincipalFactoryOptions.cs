using Fake.Collections;

namespace Fake.Identity.Security.Claims;

public class FakeClaimsPrincipalFactoryOptions
{
    public ITypeList<IFakeClaimsPrincipalContributor> Contributors { get; }

    public FakeClaimsPrincipalFactoryOptions()
    {
        Contributors = new TypeList<IFakeClaimsPrincipalContributor>();
    }
}