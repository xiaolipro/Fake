namespace Fake.AspNetCore.Http;

/// <summary>
/// 远程服务异常模型
/// </summary>
[Serializable]
public class RemoteServiceErrorModel
{
    /// <summary>
    /// 异常消息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 异常堆栈
    /// </summary>
    public string? StackTrace { get; set; }

    public object? ValidationErrors { get; set; }

    public RemoteServiceErrorModel()
    {
    }

    public RemoteServiceErrorModel(string message, string? stackTrace = null)
    {
        Message = message;
        StackTrace = stackTrace;
    }
}