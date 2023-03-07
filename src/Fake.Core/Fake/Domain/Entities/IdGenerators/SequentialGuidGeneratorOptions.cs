namespace Fake.Domain.Entities.IdGenerators;

public class SequentialGuidGeneratorOptions
{
    /// <summary>
    /// 有序Guid类型，默认SequentialAtEnd
    /// </summary>
    public SequentialGuidType SequentialGuidType { get; set; }


    public SequentialGuidGeneratorOptions(SequentialGuidType sequentialGuidType = SequentialGuidType.SequentialAsBinaryAtEnd)
    {
        SequentialGuidType = sequentialGuidType;
    }
}