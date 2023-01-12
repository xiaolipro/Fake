namespace System;

public static class FakeStringExtensions
{
    /// <summary>
    /// Indicates whether this string is null or an System.String.Empty string.
    /// </summary>
    [ContractAnnotation("str:null => true")]
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// indicates whether this string is null, empty, or consists only of white-space characters.
    /// </summary>
    [ContractAnnotation("str:null => true")]
    public static bool IsNullOrWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }
}