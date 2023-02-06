using System;

namespace Fake.UnitOfWork;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
public class UnitOfWorkAttribute : Attribute
{
    /// <summary>
    /// 启用工作单元，默认: true
    /// </summary>
    public bool IsEnabled { get; set; }
}