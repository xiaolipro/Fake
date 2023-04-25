namespace Fake.Domain.Repositories;

public class PaginatedListResponse<T> where T:class
{
    public PaginatedListResponse(int totalCount, int totalPages, IReadOnlyList<T> items)
    {
        TotalCount = totalCount;
        TotalPages = totalPages;
        Items = items;
    }

    public int TotalCount { get; }
    
    public int TotalPages { get; }

    public IReadOnlyList<T> Items { get; }
}