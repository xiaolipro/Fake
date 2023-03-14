using System.Collections.Generic;
using Microsoft.Extensions.Localization;

namespace Fake.Localization;

public class SimpleLocalizedStringContainer : ILocalizedStringContainer
{
    private readonly Dictionary<string, LocalizedString> _dictionary;
    public string Path { get; }
    public string CultureName { get; }

    public SimpleLocalizedStringContainer(string path, string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        _dictionary = dictionary;
        Path = path;
        CultureName = cultureName;
    }

    public LocalizedString GetLocalizedStringOrDefault(string name)
    {
        return _dictionary.GetOrDefault(name);
    }

    public void Fill(Dictionary<string, LocalizedString> dictionary)
    {
        // 追加
        foreach (var item in _dictionary)
        {
            dictionary[item.Key] = item.Value;
        }
    }
}