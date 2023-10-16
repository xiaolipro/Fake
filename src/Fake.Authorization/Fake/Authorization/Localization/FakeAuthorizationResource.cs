using Fake.Localization;

namespace Fake.Authorization.Localization;

[LocalizationResourceName("FakeAuthorization")]
public class FakeAuthorizationResource
{
    public static readonly string GivenPolicyHasNotGranted = "Fake.Authorization:010001";

    public static readonly string GivenPolicyHasNotGrantedWithPolicyName = "Fake.Authorization:010002";

    public static readonly string GivenPolicyHasNotGrantedForGivenResource = "Fake.Authorization:010003";

    public static readonly string GivenRequirementHasNotGrantedForGivenResource = "Fake.Authorization:010004";

    public static readonly string GivenRequirementsHasNotGrantedForGivenResource = "Fake.Authorization:010005";
}