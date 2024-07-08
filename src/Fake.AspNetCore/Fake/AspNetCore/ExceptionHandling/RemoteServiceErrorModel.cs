namespace Fake.AspNetCore.ExceptionHandling;

/// <summary>
/// 远程服务异常模型
/// </summary>
[Serializable]
public class RemoteServiceErrorModel
{
    /// <summary>
    /// 异常代码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 异常信息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 异常明细
    /// </summary>
    public string? Details { get; set; }

    public RemoteServiceErrorModel()
    {
    }

    public RemoteServiceErrorModel(string code, string? message = null)
    {
        Code = code;
        Message = message;
    }
}