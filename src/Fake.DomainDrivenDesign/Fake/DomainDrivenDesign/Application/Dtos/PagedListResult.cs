namespace Fake.DomainDrivenDesign.Application.Dtos;

public class PagedListResult<T>(List<T> items, int totalCount)
{
    public int TotalCount { get; set; } = totalCount;

    public IReadOnlyList<T> Items { get; set; } = items;

    public PagedListResult() : this(new List<T>(), 0)
    {
    }
}