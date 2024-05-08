namespace Fake.DomainDrivenDesign.Application.Dtos;

public class UnifyPagedResultDto<T> : UnifyResultDto<List<T>>
{
    public int TotalCount { get; set; }
}