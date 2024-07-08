using Fake.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fake.Domain.Services;

public class DomainService : IDomainService
{
    // 属性注入，必须public
    public ILazyServiceProvider LazyServiceProvider { get; set; } = default!;

    protected ILogger Logger => LazyServiceProvider.GetService<ILogger>(provider =>
        provider.GetRequiredService<ILoggerFactory>().CreateLogger(GetType().FullName ?? string.Empty))!;
}