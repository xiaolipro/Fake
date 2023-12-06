using System.Collections.Generic;
using Microsoft.Extensions.Localization;

namespace Fake.Localization;

/// <summary>
/// 查找本地化字符串的容器。
/// </summary>
public interface ILocalizedStringContainer
{
    /// <summary>
    /// 路径
    /// </summary>
    string Path { get; }

    /// <summary>
    /// 文化
    /// </summary>
    string? CultureName { get; }

    /// <summary>
    /// 查字典
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    LocalizedString? GetLocalizedStringOrDefault(string name);

    /// <summary>
    /// 填充字典
    /// </summary>
    /// <param name="dictionary"></param>
    void Fill(Dictionary<string, LocalizedString?> dictionary);
}