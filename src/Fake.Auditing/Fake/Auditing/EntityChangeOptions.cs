using System.Collections.Generic;

namespace Fake.Auditing;

public class EntityChangeOptions
{
    /// <summary>
    /// 启用实体变更审计
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 实体属性变更，新旧值最大长度
    /// </summary>
    public int ValueMaxLength { get; set; }

    /// <summary>
    /// 忽略审计属性
    /// </summary>
    public List<string> IgnoreProperties { get; set; } = [];
}