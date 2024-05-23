using System;

namespace Fake.UnitOfWork;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class DisableUnitOfWorkAttribute : Attribute;