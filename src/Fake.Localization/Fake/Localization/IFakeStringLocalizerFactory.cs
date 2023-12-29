using Microsoft.Extensions.Localization;

namespace Fake.Localization;

public interface IFakeStringLocalizerFactory : IStringLocalizerFactory
{
    IStringLocalizer? CreateDefaultOrNull();

    IStringLocalizer? CreateByResourceNameOrNull(string resourceName);
}