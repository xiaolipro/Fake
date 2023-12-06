using Fake.Localization;

namespace Microsoft.Extensions.Localization;

public static class FakeStringLocalizerFactoryExtensions
{
    public static IStringLocalizer CreateByResourceName(this IStringLocalizerFactory localizerFactory,
        string resourceName)
    {
        if (localizerFactory is IFakeStringLocalizerFactory fakeStringLocalizerFactory)
        {
            return fakeStringLocalizerFactory.CreateByResourceName(resourceName);
        }

        return null;
    }
}