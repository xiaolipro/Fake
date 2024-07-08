using System.Runtime.Serialization;
using Fake.ExceptionHandling;
using Fake.Logging;
using Microsoft.Extensions.Logging;

namespace Fake.Application;

[Serializable]
public class BusinessException : FakeException, IHasLogLevel, IHasErrorCode
{
    public string? Code { get; }

    public LogLevel LogLevel { get; set; }

    /// <summary>
    /// 用于序列化的构造函数
    /// </summary>
    public BusinessException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {
    }

    public BusinessException(
        string? code = null,
        string? message = null,
        Exception? innerException = null,
        LogLevel logLevel = LogLevel.Warning)
        : base(message, innerException)
    {
        Code = code;
        LogLevel = logLevel;
    }

    public override string StackTrace => String.Empty;
}