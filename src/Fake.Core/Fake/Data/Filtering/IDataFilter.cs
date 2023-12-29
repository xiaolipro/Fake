namespace Fake.Data.Filtering;

public interface IDataFilter
{
    IDisposable Enable<TFilter>() where TFilter : ICanDataFilter;

    IDisposable Disable<TFilter>() where TFilter : ICanDataFilter;

    bool IsEnabled<TFilter>() where TFilter : ICanDataFilter;
}

public interface IDataFilter<TFilter> where TFilter : ICanDataFilter
{
    IDisposable Enable();

    IDisposable Disable();

    bool IsEnabled { get; }
}