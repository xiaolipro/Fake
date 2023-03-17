using Fake.Localization;
using JetBrains.Annotations;

namespace Microsoft.Extensions.Localization;

public static class FakeStringLocalizerFactoryExtensions
{
    [CanBeNull]
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