using Fake.Helpers;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fake.AspNetCore.Mvc.ApiExplorer;

public class RemoteServiceApiDescriptionProvider(
    IModelMetadataProvider modelMetadataProvider,
    IOptions<MvcOptions> mvcOptions)
    : IApiDescriptionProvider
{
    // after the https://github.com/xiaolipro/aspnetcore/blob/main/src/Mvc/Mvc.ApiExplorer/src/DefaultApiDescriptionProvider.cs
    public int Order => -999;

    public void OnProvidersExecuting(ApiDescriptionProviderContext context)
    {
    }

    public void OnProvidersExecuted(ApiDescriptionProviderContext context)
    {
        foreach (var apiResponseType in GetApiResponseTypes())
        {
            foreach (var apiDescription in context.Results)
            {
                if (apiDescription.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
                {
                    continue;
                }

                if (!controllerActionDescriptor.ControllerTypeInfo.IsAssignableTo<IRemoteService>())
                {
                    continue;
                }

                // ProducesResponseTypeAttribute优先级大于全局配置
                var actionProducesResponseTypeAttributes = ReflectionHelper
                    .GetAttributes<ProducesResponseTypeAttribute>(controllerActionDescriptor.MethodInfo);
                if (actionProducesResponseTypeAttributes.Any(x => x.StatusCode == apiResponseType.StatusCode))
                {
                    continue;
                }

                var actionResponseType = apiDescription.SupportedResponseTypes.FirstOrDefault(x =>
                    x.StatusCode == apiResponseType.StatusCode);
                if (actionResponseType != default)
                {
                    apiDescription.SupportedResponseTypes.Remove(actionResponseType);
                }

                apiDescription.SupportedResponseTypes.Add(apiResponseType);
            }
        }
    }

    protected virtual IEnumerable<ApiResponseType> GetApiResponseTypes()
    {
        return [];
    }
}