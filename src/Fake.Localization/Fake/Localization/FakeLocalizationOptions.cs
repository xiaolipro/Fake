using System;
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
    /// 1.先从全局贡献者获取（倒序）
    /// </summary>
    public ITypeList<ILocalizationResourceContributor> GlobalContributors { get; } =
        new TypeList<ILocalizationResourceContributor>();

    /// <summary>
    /// 2.如果指定culture找不到，尝试从parent culture找
    /// </summary>
    public bool TryGetFromParentCulture { get; set; }

    /// <summary>
    /// 3.如果parent culture也找不到，尝试从default culture找
    /// </summary>
    public bool TryGetFromDefaultCulture { get; set; }

    /// <summary>
    /// 默认culture，优先级低于<see cref="LocalizationResourceBase"/>
    /// </summary>
    public string DefaultCulture { get; set; } = "zh";
}