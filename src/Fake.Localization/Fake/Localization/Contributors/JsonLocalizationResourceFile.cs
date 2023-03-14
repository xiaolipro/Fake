using System.Collections.Generic;

namespace Fake.Localization.Contributors;

public class JsonLocalizationResourceFile
{
    public string Culture { get; set; }
    public Dictionary<string, string> Texts { get; set; }

    public JsonLocalizationResourceFile()
    {
        Texts = new Dictionary<string, string>();
    }
}