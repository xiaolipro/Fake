using JetBrains.Annotations;
using Microsoft.Extensions.Localization;

namespace Fake.Localization;

public interface IFakeStringLocalizerFactory
{
    [CanBeNull]
    IStringLocalizer CreateByResourceName(string resourceName);
}