namespace Fake.Data.Filtering;

public class DataFilterState(bool isEnabled)
{
    public bool IsEnabled { get; set; } = isEnabled;

    public DataFilterState Clone()
    {
        return new DataFilterState(IsEnabled);
    }
}