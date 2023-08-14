using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Fake.Consul;

public class ConsulHostedService : IHostedService
{
    private readonly ILogger<ConsulHostedService> _logger;
    private readonly IConsulClient _consulClient;
    private readonly FakeConsulRegisterOptions _fakeConsulRegisterOptions;
    private CancellationTokenSource _consulCancellationToken;
    private string _serviceId;

    public ConsulHostedService(ILogger<ConsulHostedService> logger, IConsulClient consulClient,
        IOptions<FakeConsulRegisterOptions> options)
    {
        _logger = logger;
        _consulClient = consulClient;
        _fakeConsulRegisterOptions = options.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Create a linked token so we can trigger cancellation outside of this token's cancellation
        _consulCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        #region 服务注销，防止重复注册

        var services = await _consulClient.Catalog.Service(_fakeConsulRegisterOptions.ServiceName, cancellationToken);

        var targets = services.Response.Where(x =>
            x.ServiceAddress == _fakeConsulRegisterOptions.Host && x.ServicePort == _fakeConsulRegisterOptions.Port);

        foreach (var service in targets)
        {
            await _consulClient.Agent.ServiceDeregister(service.ServiceID, cancellationToken);
        }

        #endregion


        var registration = BuildRegistration();
        await _consulClient.Agent.ServiceRegister(registration, cancellationToken);
        _logger.LogInformation("Consul注册已完成 {Message}", JsonConvert.SerializeObject(_fakeConsulRegisterOptions));
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _consulCancellationToken.Cancel();
        await _consulClient.Agent.ServiceDeregister(_serviceId, cancellationToken);
        _logger.LogInformation("Consul已注销");
    }


    private AgentServiceRegistration BuildRegistration()
    {
        var supportGrpc = _fakeConsulRegisterOptions.GrpcPort != 0;
        var httpHealthCheck = new AgentServiceCheck()
        {
            Interval = TimeSpan.FromSeconds(_fakeConsulRegisterOptions.Interval), // 健康检查时间间隔
            HTTP =
                $"{_fakeConsulRegisterOptions.Host}:{_fakeConsulRegisterOptions.Port}/{_fakeConsulRegisterOptions.HealthCheckPath.Trim('/')}",
            Timeout = TimeSpan.FromSeconds(_fakeConsulRegisterOptions.Timeout), // 心跳超时时间
            DeregisterCriticalServiceAfter =
                TimeSpan.FromSeconds(_fakeConsulRegisterOptions.DeregisterTime), // 服务挂掉多久后注销，这个不配置挂掉的节点会一直在
        };

        _serviceId =
            $"{_fakeConsulRegisterOptions.Host}:{_fakeConsulRegisterOptions.Port}  Start in {DateTime.Now} {(supportGrpc ? $"SupportGrpc in {_fakeConsulRegisterOptions.GrpcPort}" : "")}";
        var registration = new AgentServiceRegistration()
        {
            ID = _serviceId, // 服务唯一Id
            Name = _fakeConsulRegisterOptions.ServiceName, // 服务组名称
            Address = _fakeConsulRegisterOptions.Host, // 服务主机
            Port = _fakeConsulRegisterOptions.Port, // 服务端口
            Tags = _fakeConsulRegisterOptions.Tags, // 一组标签
            Check = httpHealthCheck,
            Meta = new Dictionary<string, string>()
            {
                { nameof(_fakeConsulRegisterOptions.Weight), _fakeConsulRegisterOptions.Weight.ToString() },
            } // 元数据
        };

        // grpc心跳
        if (supportGrpc)
        {
            registration.Check.GRPC =
                $"{_fakeConsulRegisterOptions.Host}:{_fakeConsulRegisterOptions.GrpcPort}/{_fakeConsulRegisterOptions.GrpcHealthCheckPath?.Trim('/')}";
            registration.Check.GRPCUseTLS = false; // consul在内网，不需要tls
        }

        return registration;
    }
}