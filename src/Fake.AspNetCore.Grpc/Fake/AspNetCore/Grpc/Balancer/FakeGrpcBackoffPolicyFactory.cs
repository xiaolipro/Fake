using Grpc.Net.Client.Balancer;
using Microsoft.Extensions.Options;

namespace Fake.AspNetCore.Grpc.Balancer
{
    /// <summary>
    /// 为Resolver提供回退延迟
    /// 解决后台任务（客户端）先起，服务端未起的问题，客户端解析不到服务，会根据回退策略提供的时间，不断重试。
    /// </summary>
    public class FakeGrpcBackoffPolicyFactory : IBackoffPolicyFactory
    {
        private readonly FakeGrpcClientOptions _options;
        public FakeGrpcBackoffPolicyFactory(IOptions<FakeGrpcClientOptions> options)
        {
            _options = options.Value;
        }
        public IBackoffPolicy Create()
        {
            return new FakeGrpcBackoffPolicy(_options);
        }
    }
}
