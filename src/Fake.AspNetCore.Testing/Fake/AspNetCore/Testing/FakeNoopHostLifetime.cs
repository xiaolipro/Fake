using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Fake.AspNetCore.Testing;

/// <summary>
/// 无操作（No-op）主机生命周期。
/// 将主机的生命周期设置为一个空操作（No-op），即不做任何处理。
/// 他只是fake的Microsoft.AspNetCore.TestHost命名空间下的NoopHostLifetime类，因为我们需要重新定义生命周期。
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