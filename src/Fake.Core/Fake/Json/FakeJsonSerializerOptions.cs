namespace Fake.Json;

public class FakeJsonSerializerOptions
{
    /// <summary>
    /// 支持解析的日期格式
    /// </summary>
    public List<string> InputDateTimeFormats { get; set; }

    /// <summary>
    /// 统一返回的日期格式
    /// </summary>
    public string OutputDateTimeFormat { get; set; }
    
    /// <summary>
    /// 是否将long类型转换为string
    /// </summary>
    public bool LongToString { get; set; }

    /// <summary>
    /// 尝试将"true"/"false"转化为等价的bool值（忽略大小写）
    /// </summary>
    public bool StringToBoolean { get; set; }

    public FakeJsonSerializerOptions()
    {
        InputDateTimeFormats = new List<string>();
    }
}