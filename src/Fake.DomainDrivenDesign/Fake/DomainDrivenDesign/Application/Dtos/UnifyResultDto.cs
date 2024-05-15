using System.Net;

namespace Fake.DomainDrivenDesign.Application.Dtos;

public class UnifyResultDto(object? data = null)
{
    public string Code { get; set; } = HttpStatusCode.OK.ToString();

    public string Message { get; set; } = "操作成功";

    public object? Data { get; set; } = data;
}

public class UnifyResultDto<T>(T? data = default) : UnifyResultDto(data);