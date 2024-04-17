using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Fake.AspNetCore.Endpoint;

public interface IEndpointServiceMapper
{
    void MapEndpoint(IEndpointRouteBuilder builder, IEndpointService service);
}

public interface IEndpointServiceHelper
{
    List<MethodInfo> GetEndpointMethods(Type serviceType);
}

public class EndpointServiceMapper : IEndpointServiceMapper
{
    private readonly IEndpointServiceHelper _serviceHelper;

    public EndpointServiceMapper(IEndpointServiceHelper serviceHelper)
    {
        _serviceHelper = serviceHelper;
    }

    public void MapEndpoint(IEndpointRouteBuilder builder, IEndpointService service)
    {
        var serviceType = service.GetType();

        var methodInfos = _serviceHelper.GetEndpointMethods(serviceType);

        var groupEndpointFilter = serviceType.GetCustomAttributes(true).OfType<IEndpointFilter>();
        var groupRoute = serviceType.GetCustomAttributes<RouteAttribute>(true);
        foreach (var methodInfo in methodInfos)
        {
            var handler = CreateDelegate(methodInfo, service);
            var route = methodInfo.GetCustomAttributes<RouteAttribute>(true);
        }
    }

    private Delegate CreateDelegate(MethodInfo methodInfo, object targetInstance)
    {
        var type = Expression.GetDelegateType(methodInfo.GetParameters()
            .Select(parameterInfo => parameterInfo.ParameterType)
            .Concat(new List<Type>
            {
                methodInfo.ReturnType
            }).ToArray());
        return Delegate.CreateDelegate(type, targetInstance, methodInfo);
    }
}