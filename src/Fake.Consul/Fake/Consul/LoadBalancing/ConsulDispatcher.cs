using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Fake.Consul.LoadBalancing
{
    /// <summary>
    /// ConsulDispatcher基类
    /// </summary>
    public abstract class ConsulDispatcher : IDispatcher
    {
        private readonly ILogger<ConsulDispatcher> _logger;
        private readonly IResolver _resolver;
        private readonly IBalancer _balancer;

        public ConsulDispatcher(ILogger<ConsulDispatcher> logger, IResolver resolver, IBalancer balancer)
        {
            _logger = logger;
            _resolver = resolver;
            _balancer = balancer;
        }

        /// <summary>
        /// 根据服务名称获取调度后的真实地址
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public async Task<string> GetRealHostAsync(string serviceName)
        {
            return await ChooseHostAsync(serviceName);
        }

        /// <summary>
        /// 根据服务选择一台主机
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>主机：IP+Port</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        private async Task<string> ChooseHostAsync(string serviceName)
        {
            var (uris, metaData) = await _resolver.ResolutionService(serviceName);

            IEnumerable<int> weights = default;
            if (_balancer.GetType() == typeof(WeightBalancer))
            {
                weights = (metaData as List<Dictionary<string,string>>)?.Select(x => int.TryParse(x["Weight"], out int count) ? count : 1);
            }

            int index = _balancer.Pick(uris.Count, weights);


            if (index >= uris.Count) throw new IndexOutOfRangeException();

            var service = uris[index];
            return $"{service.Host}:{service.Port}";
        }
    }
}