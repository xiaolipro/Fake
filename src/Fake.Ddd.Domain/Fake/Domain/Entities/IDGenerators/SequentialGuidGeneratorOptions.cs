namespace Fake.Domain.Entities.IDGenerators;

public class SequentialGuidGeneratorOptions
{
    /// <summary>
    /// 有序Guid类型，默认SequentialAtEnd
    /// </summary>
    public SequentialGuidType SequentialGuidType { get; set; }
}