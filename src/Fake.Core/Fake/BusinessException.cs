using System.Runtime.Serialization;
using Fake.ExceptionHandling;
using Fake.Logging;
using Microsoft.Extensions.Logging;

namespace Fake;

[Serializable]
public class BusinessException : FakeException, IHasErrorCode, IHasErrorDetails, IHasLogLevel
{
    public string? Code { get; }
    public string? Details { get; }
    public LogLevel LogLevel { get; set; }

    /// <summary>
    /// 用于序列化的构造函数
    /// </summary>
    public BusinessException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {
    }

    public BusinessException(
        string? message = null,
        string? code = null,
        string? details = null,
        Exception? innerException = null,
        LogLevel logLevel = LogLevel.Warning)
        : base(message, innerException)
    {
        Code = code;
        Details = details;
        LogLevel = logLevel;
    }
}