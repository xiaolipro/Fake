namespace Fake.Data.Filtering;

public class FakeDataFilterOptions
{
    private readonly Dictionary<Type, DataFilterState> _defaultStates = new();

    /// <summary>
    /// 是否启用默认过滤器
    /// </summary>
    public bool IsDefaultEnabled { get; set; } = true;

    public DataFilterState? GetDataFilterStateOrNull<T>() where T : ICanFilter
    {
        return _defaultStates.GetOrDefault(typeof(T));
    }

    public void SetDefaultState<T>(bool defaultState) where T : ICanFilter
    {
        _defaultStates[typeof(T)] = new DataFilterState(defaultState);
    }
}