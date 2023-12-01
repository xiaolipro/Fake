using System;

namespace Fake.Auditing;

/// <summary>
/// 表示开启审计
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class AuditedAttribute : Attribute;