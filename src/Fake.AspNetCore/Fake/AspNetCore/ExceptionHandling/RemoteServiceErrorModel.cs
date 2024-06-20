namespace Fake.AspNetCore.ExceptionHandling;

/// <summary>
/// 远程服务异常模型
/// </summary>
[Serializable]
public class RemoteServiceErrorModel
{
    /// <summary>
    /// 异常类型
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// 异常信息
    /// </summary>
    public string? Message { get; set; }

    public RemoteServiceErrorModel()
    {
    }

    public RemoteServiceErrorModel(string type, string? message = null)
    {
        Type = type;
        Message = message;
    }
}