using System;
using System.Threading.Tasks;

namespace Fake.Auditing;

public interface IAuditLogSaveHandle : IDisposable
{
    Task SaveAsync();
}