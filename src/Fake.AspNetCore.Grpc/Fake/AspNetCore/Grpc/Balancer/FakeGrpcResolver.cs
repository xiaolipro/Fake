using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fake.LoadBalancing;
using Grpc.Net.Client.Balancer;
using Microsoft.Extensions.Logging;

namespace Fake.AspNetCore.Grpc.Balancer;

internal class FakeGrpcResolver : PollingResolver
{
    private readonly Uri _address;
    private readonly ILogger _logger;
    private readonly IFakeBalancer _balancer;
    private readonly FakeGrpcClientOptions _options;
    private Timer _timer;

    public FakeGrpcResolver(ILoggerFactory loggerFactory, IBackoffPolicyFactory backoffPolicyFactory, Uri address,
        IFakeBalancer balancer, FakeGrpcClientOptions options) : base(loggerFactory, backoffPolicyFactory)
    {
        _logger = loggerFactory.CreateLogger(typeof(FakeGrpcResolver));
        if (string.IsNullOrWhiteSpace(address.Host)) throw new ArgumentNullException(nameof(address));
        _address = address;
        _balancer = balancer;
        _options = options;
    }


    protected override async Task ResolveAsync(CancellationToken cancellationToken)
    {
        // 解析服务
        await _balancer.ReferenceAsync(_address.Host);

        // 防止服务端没起的时候重复请求服务解析
        if (_balancer.ServiceNodes == null || _balancer.ServiceNodes.Count < 1) return;

        var addresses = _balancer.ServiceNodes[_address.Host].Select(h =>
        {
            // todo：这里的grpc端口号需要从配置中获取
            return new BalancerAddress(h, 110);
        }).ToArray();

        // 将结果传递回通道。
        Listener(ResolverResult.ForResult(addresses));
    }

    protected override void OnStarted()
    {
        base.OnStarted();

        if (_options.RefreshInterval != Timeout.InfiniteTimeSpan)
        {
            _timer = new Timer(OnTimerCallback, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            _timer.Change(_options.RefreshInterval, _options.RefreshInterval);
        }
    }

    private void OnTimerCallback(object state)
    {
        try
        {
            _logger.LogInformation("{TotalSeconds}s刷新解析", _options.RefreshInterval.TotalSeconds);
            Refresh();
        }
        catch (Exception)
        {
            _logger.LogError("服务解析器刷新失败");
        }
    }
}