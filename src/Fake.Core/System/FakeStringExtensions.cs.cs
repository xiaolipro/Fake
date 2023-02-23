namespace System;

public static class FakeStringExtensions
{
    /// <summary>
    /// 表示此字符串 是 null或空字符串。
    /// </summary>
    [ContractAnnotation("str:null => true")]
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }
    
    /// <summary>
    /// 表示此字符串 不是 null或空字符串。
    /// </summary>
    [ContractAnnotation("str:null => true")]
    public static bool NotNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// 表示此字符串 是 null或空字符串或空白格。
    /// </summary>
    [ContractAnnotation("str:null => true")]
    public static bool IsNullOrWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }
    
    /// <summary>
    /// 表示此字符串 不是 null或空字符串或空白格。
    /// </summary>
    [ContractAnnotation("str:null => true")]
    public static bool NotNullOrWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }
}