using System.Collections.Generic;
using System.Linq;
using Fake.LoadBalancing;
using Grpc.Net.Client.Balancer;
using Microsoft.Extensions.Logging;

namespace Fake.AspNetCore.Grpc.Balancer;

internal class FakeGrpcBalancer : SubchannelsLoadBalancer
{
    private readonly ILogger _logger;
    private readonly IServiceBalancer _serviceBalancer;

    public FakeGrpcBalancer(IChannelControlHelper controller, ILoggerFactory loggerFactory, IServiceBalancer serviceBalancer)
        : base(controller, loggerFactory)
    {
        _logger = loggerFactory.CreateLogger(typeof(FakeGrpcBalancer));
        _serviceBalancer = serviceBalancer;
    }


    protected override SubchannelPicker CreatePicker(IReadOnlyList<Subchannel> readySubchannels)
    {
        return new CustomPicker(readySubchannels, _serviceBalancer, _logger);
    }

    private class CustomPicker : SubchannelPicker
    {
        private readonly IReadOnlyList<Subchannel> _subchannels;
        private readonly IServiceBalancer _serviceBalancer;
        private readonly ILogger _logger;

        public CustomPicker(IReadOnlyList<Subchannel> subchannels, IServiceBalancer serviceBalancer, ILogger logger)
        {
            _subchannels = subchannels;
            _serviceBalancer = serviceBalancer;
            _logger = logger;
        }

        public override PickResult Pick(PickContext context)
        {
            var address = _serviceBalancer.Pick(context.Request!.RequestUri!.Host, true);
            var channel = _subchannels.FirstOrDefault(x => x.ToString() == address);

            if (channel == null)
            {
                throw new FakeException($"找不到可用的服务：{address}");
            }

            _logger.LogInformation("来自{BalancerName}均衡器{Count}选1的结果：{ChannelCurrentAddress}",
                _serviceBalancer.GetType().Name, _subchannels.Count, channel.CurrentAddress);
            // Pick a sub-channel.
            var res = PickResult.ForSubchannel(channel);

            _logger.LogInformation("Status----{Status}", res.Status.ToString());

            return res;
        }
    }
}