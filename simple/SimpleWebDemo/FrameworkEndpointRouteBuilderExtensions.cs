using Fake;
using Fake.DependencyInjection;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

public static class FrameworkEndpointRouteBuilderExtensions
{
    public static IEndpointConventionBuilder MapFramework(this IEndpointRouteBuilder endpoints,
        Action<FrameworkConfigurationBuilder> configure)
    {
        ThrowHelper.ThrowIfNull(endpoints);
        ThrowHelper.ThrowIfNull(configure);

        var dataSource = endpoints.ServiceProvider.GetRequiredService<FrameworkEndpointDataSource>();

        var configurationBuilder = new FrameworkConfigurationBuilder(dataSource);
        configure(configurationBuilder);

        endpoints.DataSources.Add(dataSource);

        return dataSource;
    }
}

public class FrameworkConfigurationBuilder
{
    private readonly FrameworkEndpointDataSource _dataSource;

    internal FrameworkConfigurationBuilder(FrameworkEndpointDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public void AddPattern(string pattern)
    {
        AddPattern(RoutePatternFactory.Parse(pattern));
    }

    public void AddPattern(RoutePattern pattern)
    {
        _dataSource.Patterns.Add(pattern);
    }

    public void AddHubMethod(string hub, string method, RequestDelegate requestDelegate)
    {
        _dataSource.HubMethods.Add(new HubMethod
        {
            Hub = hub,
            Method = method,
            RequestDelegate = requestDelegate
        });
    }
}


internal class FrameworkEndpointDataSource : EndpointDataSource, IEndpointConventionBuilder, ISingletonDependency
{
    private readonly RoutePatternTransformer _routePatternTransformer;
    private readonly List<Action<EndpointBuilder>> _conventions;

    public List<RoutePattern> Patterns { get; }
    public List<HubMethod> HubMethods { get; }

    private List<Endpoint> _endpoints;

    public FrameworkEndpointDataSource(RoutePatternTransformer routePatternTransformer)
    {
        _routePatternTransformer = routePatternTransformer;
        _conventions = new List<Action<EndpointBuilder>>();

        Patterns = new List<RoutePattern>();
        HubMethods = new List<HubMethod>();
    }

    public override IReadOnlyList<Endpoint> Endpoints
    {
        get
        {
            if (_endpoints == null)
            {
                _endpoints = BuildEndpoints();
            }

            return _endpoints;
        }
    }

    private List<Endpoint> BuildEndpoints()
    {
        List<Endpoint> endpoints = new List<Endpoint>();

        foreach (var hubMethod in HubMethods)
        {
            var requiredValues = new { hub = hubMethod.Hub, method = hubMethod.Method };
            var order = 1;

            foreach (var pattern in Patterns)
            {
                var resolvedPattern = _routePatternTransformer.SubstituteRequiredValues(pattern, requiredValues);
                if (resolvedPattern == null)
                {
                    continue;
                }

                var endpointBuilder = new RouteEndpointBuilder(
                    hubMethod.RequestDelegate,
                    resolvedPattern,
                    order++);
                endpointBuilder.DisplayName = $"{hubMethod.Hub}.{hubMethod.Method}";

                foreach (var convention in _conventions)
                {
                    convention(endpointBuilder);
                }

                endpoints.Add(endpointBuilder.Build());
            }
        }

        return endpoints;
    }

    public override IChangeToken GetChangeToken()
    {
        return NullChangeToken.Singleton;
    }

    public void Add(Action<EndpointBuilder> convention)
    {
        _conventions.Add(convention);
    }
}

internal class HubMethod
{
    public string Hub { get; set; }
    public string Method { get; set; }
    public RequestDelegate RequestDelegate { get; set; }
}