namespace Fake.AspNetCore.ExceptionHandling;

/// <summary>
/// 远程服务异常模型
/// </summary>
[Serializable]
public class RemoteServiceErrorModel
{
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

    public RemoteServiceErrorModel(string message, string? details = null)
    {
        Message = message;
        Details = details;
    }
}