using System.Collections;

namespace Fake.AspNetCore.Http;

/// <summary>
/// 远程服务异常模型
/// </summary>
[Serializable]
public class RemoteServiceErrorModel
{
    /// <summary>
    /// 异常code
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 异常消息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 异常明细
    /// </summary>
    public string? StackTrace { get; set; }

    /// <summary>
    /// 校验异常
    /// </summary>
    public string? ValidationErrorMessage { get; set; }

    /// <summary>
    /// 自定义数据
    /// </summary>
    public IDictionary? Data { get; set; }

    public RemoteServiceErrorModel()
    {
    }

    public RemoteServiceErrorModel(string message, string? stackTrace = null, string? code = null,
        IDictionary? data = null)
    {
        Message = message;
        StackTrace = stackTrace;
        Code = code;
        Data = data;
    }
}