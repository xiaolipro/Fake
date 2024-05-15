namespace Fake.DomainDrivenDesign.Application.Dtos;

public class PagedListDto<T>(List<T> items, int totalCount)
{
    public int TotalCount { get; set; } = totalCount;

    public new List<T> Items { get; set; } = items;

    public PagedListDto() : this(new List<T>(), 0)
    {
    }
}