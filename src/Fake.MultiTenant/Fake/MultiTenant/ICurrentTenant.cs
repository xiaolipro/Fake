using System;

namespace Fake.MultiTenant;

public interface ICurrentTenant
{
    bool Is
    Guid Id { get; }

    string Name { get; }
}