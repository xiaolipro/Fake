using Fake.Collections;
using Fake.Localization.Contributors;

namespace Fake.Localization;

public class FakeLocalizationOptions
{
    public FakeLocalizationOptions()
    {
        GlobalContributors = new TypeList<ILocalizationResourceContributor>();
        Resources = new LocalizationResourceDictionary();
    }

    /// <summary>
    /// 本地化资源字典
    /// </summary>
    public LocalizationResourceDictionary Resources { get; }

    /// <summary>
    /// 1.先从全局贡献者获取（倒序）
    /// </summary>
    public ITypeList<ILocalizationResourceContributor> GlobalContributors { get; }

    /// <summary>
    /// 2.如果指定culture找不到，尝试从parent culture找
    /// </summary>
    public bool TryGetFromParentCulture { get; set; }

    /// <summary>
    /// 3.如果parent culture也找不到，尝试从default culture找
    /// </summary>
    public bool TryGetFromDefaultCulture { get; set; }

    /// <summary>
    /// 默认culture，优先级低于<see cref="AbstractLocalizationResource"/>
    /// </summary>
    public string DefaultCulture { get; set; }
}