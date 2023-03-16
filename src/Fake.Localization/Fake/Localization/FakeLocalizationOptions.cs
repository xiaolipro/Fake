﻿using Fake.Collections;
using Fake.Localization.Contributors;

namespace Fake.Localization;

public class FakeLocalizationOptions
{
    /// <summary>
    /// 本地化资源字典
    /// </summary>
    public LocalizationResourceDictionary Resources { get; }
    
    /// <summary>
    /// 全局贡献者
    /// </summary>
    public ITypeList<ILocalizationResourceContributor> GlobalContributors { get; }

    /// <summary>
    /// 默认culture
    /// </summary>
    public string DefaultCulture { get; set; }

    /// <summary>
    /// 如果指定culture找不到，尝试从parent culture找
    /// </summary>
    public bool TryGetFromParentCulture { get; set; }

    /// <summary>
    /// 如果parent culture也找不到，尝试从default culture找
    /// </summary>
    public bool TryGetFromDefaultCulture { get; set; }


    public FakeLocalizationOptions()
    {
        GlobalContributors = new TypeList<ILocalizationResourceContributor>();
        Resources = new LocalizationResourceDictionary();
    }
}