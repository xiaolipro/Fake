using System.Collections.Generic;

namespace Fake.Localization.Contributors;

public class JsonVirtualLocalizationResourceFile
{
    public string? Culture { get; set; }
    public Dictionary<string, string> Texts { get; set; } = new();
}