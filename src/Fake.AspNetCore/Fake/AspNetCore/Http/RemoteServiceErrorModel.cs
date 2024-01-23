using System;
using System.Collections;
using System.Collections.Generic;

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
    public string? Details { get; set; }

    /// <summary>
    /// 自定义数据
    /// </summary>
    public IDictionary? Data { get; set; }

    /// <summary>
    /// 校验异常
    /// </summary>
    public List<RemoteServiceValidationErrorModel>? ValidationErrors { get; set; }

    public RemoteServiceErrorModel()
    {
    }

    public RemoteServiceErrorModel(string message, string? details = null, string? code = null,
        IDictionary? data = null)
    {
        Message = message;
        Details = details;
        Code = code;
        Data = data;
    }
}