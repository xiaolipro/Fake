using Microsoft.Extensions.Localization;

namespace Fake.Localization;

public interface IFakeStringLocalizerFactory
{
    IStringLocalizer CreateByResourceName(string resourceName);
}