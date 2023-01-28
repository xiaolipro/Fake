using System;

namespace Fake.Auditing;

[AttributeUsage(AttributeTargets.Class| AttributeTargets.Method| AttributeTargets.Property)]
public class AuditedAttribute:Attribute{}