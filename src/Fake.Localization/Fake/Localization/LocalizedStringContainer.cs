using System.Collections.Generic;
using Microsoft.Extensions.Localization;

namespace Fake.Localization;

public class LocalizedStringContainer(
    string path,
    string cultureName,
    Dictionary<string, LocalizedString> dictionary)
    : ILocalizedStringContainer
{
    public string Path { get; } = path;
    public string CultureName { get; } = cultureName;

    public LocalizedString? GetLocalizedStringOrNull(string name)
    {
        return dictionary.GetOrDefault(name);
    }

    public void Fill(Dictionary<string, LocalizedString> dictionary1)
    {
        // 追加
        foreach (var item in dictionary)
        {
            dictionary1[item.Key] = item.Value;
        }
    }
}