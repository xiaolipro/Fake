using System;
using System.Threading;
using System.Threading.Tasks;
using Fake.LoadBalancing;
using Grpc.Net.Client.Balancer;
using Microsoft.Extensions.Logging;

namespace Fake.AspNetCore.Grpc.Balancer
{
    public class FakeResolverFactory : ResolverFactory
    {
        private readonly IFakeResolver _resolver;
        private readonly IBackoffPolicyFactory _backoffPolicyFactory;

        public override string Name => _resolver.GetType().Name;

        public FakeResolverFactory(IFakeResolver resolver)
        {
            _resolver = resolver;
        }

        public override Resolver Create(ResolverOptions options)
        {
            return new CustomResolver(options.LoggerFactory, _backoffPolicyFactory, _resolver, options.Address);
        }

        private class CustomResolver : PollingResolver
        {
            private readonly Uri _address;
            private readonly ILogger _logger;
            private readonly ILoadBalancer _resolver;
            private Timer _timer;

            public CustomResolver(ILoggerFactory loggerFactory, IBackoffPolicyFactory backoffPolicyFactory,
                ILoadBalancer resolver, Uri address) : base(loggerFactory, backoffPolicyFactory)
            {
                _logger = loggerFactory.CreateLogger(typeof(CustomResolver));
                _resolver = resolver;
                if (string.IsNullOrWhiteSpace(address.Host)) throw new ArgumentNullException(nameof(address));
                _address = address;
            }


            protected override Task ResolveAsync(CancellationToken cancellationToken)
            {
                // 防止服务端没起的时候重复请求服务解析
                if (_resolver.BalancerAddresses == null || _resolver.BalancerAddresses.Count < 1) return Task.CompletedTask;

                var addresses = uris.Select(uri => new BalancerAddress(uri.Host, uri.Port)).ToArray();

                // 将结果传递回通道。
                Listener(ResolverResult.ForResult(addresses));
                return Task.CompletedTask;
            }

            protected override void OnStarted()
            {
                base.OnStarted();

                if (_options.RefreshInterval != Timeout.InfiniteTimeSpan)
                {
                    _timer = new Timer(OnTimerCallback, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
                    _timer.Change(_resolver.RefreshInterval, _resolver.RefreshInterval);
                }
            }

            private void OnTimerCallback(object state)
            {
                try
                {
                    _logger.LogInformation("{TotalSeconds}s刷新解析", _resolver.RefreshInterval.TotalSeconds);
                    Refresh();
                }
                catch (Exception)
                {
                    _logger.LogError("服务解析器刷新失败");
                }
            }
        }
    }

    public interface IFakeResolver
    {
    }
}
