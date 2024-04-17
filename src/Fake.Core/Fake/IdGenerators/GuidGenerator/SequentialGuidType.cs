namespace Fake.IdGenerators.GuidGenerator;

public enum SequentialGuidType
{
    /// <summary>
    /// 使用Guid.ToString()后，Guid应该是有序的。
    /// </summary>
    SequentialAsString,

    /// <summary>
    /// 使用Guid.ToByteArray()后仍然有序
    /// </summary>
    SequentialAsBinaryAtStart,

    /// <summary>
    /// 有序部分在尾部
    /// </summary>
    SequentialAsBinaryAtEnd
}