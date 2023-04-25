namespace Fake.Domain.Repositories;

public class PaginatedPayload
{
    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public Dictionary<string, bool> Sorting { get; set; }
}