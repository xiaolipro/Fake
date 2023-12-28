namespace Fake.Data.Filtering;

public class FakeDataFilterOptions
{
    public Dictionary<Type, DataFilterState> DefaultStates { get; } = new();

    /// <summary>
    /// 是否启用默认过滤器
    /// </summary>
    public bool IsDefaultEnabled { get; set; } = true;
}