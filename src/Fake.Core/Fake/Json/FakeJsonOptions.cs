namespace Fake.Json;

public class FakeJsonOptions
{
    /// <summary>
    /// 输入日期格式
    /// </summary>
    public List<string> InputDateTimeFormats { get; set; }

    /// <summary>
    /// 输出日期格式
    /// </summary>
    public List<string> OutputDateTimeFormats { get; set; }

    public FakeJsonOptions()
    {
        InputDateTimeFormats = new List<string>();
    }
}