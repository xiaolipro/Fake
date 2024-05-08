namespace Fake.DomainDrivenDesign.Application.Dtos;

public class UnifyResultDto<T>(string code = "200", string message = "成功", T? data = default)
{
    public string Code { get; set; } = code;

    public string Message { get; set; } = message;

    public T? Data { get; set; } = data;
}