using System;
using System.Globalization;
using Fake.Collections;
using Fake.Localization.Contributors;

namespace Fake.Localization;

public class FakeLocalizationOptions
{
    /// <summary>
    /// 本地化资源字典
    /// </summary>
    public LocalizationResourceDictionary Resources { get; } = new();

    /// <summary>
    /// 默认资源类型
    /// </summary>
    public Type? DefaultResourceType { get; set; }

    /// <summary>
    /// 本地化贡献者（倒序）
    /// </summary>
    public ITypeList<ILocalizationResourceContributor> GlobalContributors { get; } =
        new TypeList<ILocalizationResourceContributor>();

    /// <summary>
    /// 默认culture
    /// </summary>
    public string DefaultCulture { get; set; } = CultureInfo.CurrentUICulture.Name;

    /// <summary>
    /// 尝试用默认culture的parent culture找
    /// </summary>
    public bool TryGetFromParentCulture { get; set; } = false;

    /// <summary>
    /// 尝试从本地化资源default culture找
    /// </summary>
    public bool TryGetFromDefaultCulture { get; set; } = true;
}