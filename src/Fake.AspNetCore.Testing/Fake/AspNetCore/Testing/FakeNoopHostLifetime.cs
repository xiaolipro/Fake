using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Fake.AspNetCore.Testing;

/// <summary>
/// 无操作（No-op）主机生命周期
/// </summary>
public class FakeNoopHostLifetime:IHostLifetime
{
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task WaitForStartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}