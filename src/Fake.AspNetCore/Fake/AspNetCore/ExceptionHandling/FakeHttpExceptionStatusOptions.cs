using System.Collections.Generic;
using System.Net;

namespace Fake.AspNetCore.ExceptionHandling;

public class FakeHttpExceptionStatusOptions
{
    /// <summary>
    /// 自定义异常code与http status code的映射表
    /// </summary>
    public IDictionary<string, HttpStatusCode> ErrorCodeToHttpStatusCodeMappings { get; }

    public FakeHttpExceptionStatusOptions()
    {
        ErrorCodeToHttpStatusCodeMappings = new Dictionary<string, HttpStatusCode>();
    }

    public void Map(string errorCode, HttpStatusCode httpStatusCode)
    {
        ErrorCodeToHttpStatusCodeMappings[errorCode] = httpStatusCode;
    }
}