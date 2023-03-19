namespace Fake.Domain.Entities.Auditing;

public interface IHasExtraProperties
{
    /// <summary>
    /// 额外属性
    /// </summary>
    ExtraPropertyDictionary ExtraProperties { get; }
}

[Serializable]
public class ExtraPropertyDictionary: Dictionary<string, object>
{
    public ExtraPropertyDictionary()
    {

    }

    public ExtraPropertyDictionary(IDictionary<string, object> dictionary)
        : base(dictionary)
    {
    }
}
