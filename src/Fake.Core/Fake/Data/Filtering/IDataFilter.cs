namespace Fake.Data.Filtering;

public interface IDataFilter
{
    IDisposable Enable<TFilter>() where TFilter : ICanFilter;

    IDisposable Disable<TFilter>() where TFilter : ICanFilter;

    bool IsEnabled<TFilter>() where TFilter : ICanFilter;
}

public interface IDataFilter<TFilter> where TFilter : ICanFilter
{
    IDisposable Enable();

    IDisposable Disable();

    bool IsEnabled { get; }
}