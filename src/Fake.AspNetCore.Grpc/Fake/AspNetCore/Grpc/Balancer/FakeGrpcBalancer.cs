using System.Collections.Generic;
using System.Linq;
using Fake.AspNetCore.LoadBalancing;
using Grpc.Net.Client.Balancer;
using Microsoft.Extensions.Logging;

namespace Fake.AspNetCore.Grpc.Balancer;

internal class FakeGrpcBalancer : SubchannelsLoadBalancer
{
    private readonly ILogger _logger;
    private readonly ILoadBalancer _balancer;

    public FakeGrpcBalancer(IChannelControlHelper controller, ILoggerFactory loggerFactory, ILoadBalancer balancer)
        : base(controller, loggerFactory)
    {
        _logger = loggerFactory.CreateLogger(typeof(FakeGrpcBalancer));
        _balancer = balancer;
    }


    protected override SubchannelPicker CreatePicker(IReadOnlyList<Subchannel> readySubchannels)
    {
        return new CustomPicker(readySubchannels, _balancer, _logger);
    }

    private class CustomPicker : SubchannelPicker
    {
        private readonly IReadOnlyList<Subchannel> _subchannels;
        private readonly ILoadBalancer _balancer;
        private readonly ILogger _logger;

        public CustomPicker(IReadOnlyList<Subchannel> subchannels, ILoadBalancer balancer, ILogger logger)
        {
            _subchannels = subchannels;
            _balancer = balancer;
            _logger = logger;
        }

        public override PickResult Pick(PickContext context)
        {
            var address = _balancer.Balancing();
            var channel = _subchannels.FirstOrDefault(x => x.ToString() == address);

            if (channel == null)
            {
                throw new FakeException($"找不到可用的服务：{address}");
            }

            _logger.LogInformation("来自{BalancerName}均衡器{Count}选1的结果：{ChannelCurrentAddress}",
                _balancer.GetType().Name, _subchannels.Count, channel.CurrentAddress);
            // Pick a sub-channel.
            var res = PickResult.ForSubchannel(channel);

            _logger.LogInformation("Status----{Status}", res.Status.ToString());

            return res;
        }
    }
}