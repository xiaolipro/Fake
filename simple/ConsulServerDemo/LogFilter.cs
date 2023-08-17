
namespace ConsulServerDemo;

public class LogFilter:IEndpointFilter
{
    private readonly ILogger<LogFilter> _logger;

    public LogFilter(ILogger<LogFilter> logger)
    {
        _logger = logger;
    }
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        _logger.LogInformation("方法开始执行");
        var res = await next(context);
        _logger.LogInformation("方法执行完毕");
        return res;
    }
}