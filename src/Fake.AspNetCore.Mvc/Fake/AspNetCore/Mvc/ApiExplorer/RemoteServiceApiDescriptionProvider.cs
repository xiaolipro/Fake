using System.Net;
using Fake.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

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

                var actionProducesResponseTypeAttributes = ReflectionHelper
                    .GetAttributes<ProducesResponseTypeAttribute>(controllerActionDescriptor.MethodInfo);
                if (actionProducesResponseTypeAttributes.All(x => x.StatusCode != apiResponseType.StatusCode))
                {
                    apiDescription.SupportedResponseTypes.Add(apiResponseType);
                }
            }
        }
    }

    protected virtual IEnumerable<ApiResponseType> GetApiResponseTypes()
    {
        var supportedResponseTypes = new List<int>
        {
            (int)HttpStatusCode.Forbidden,
            (int)HttpStatusCode.Unauthorized,
            (int)HttpStatusCode.BadRequest,
            (int)HttpStatusCode.NotFound,
            (int)HttpStatusCode.NotImplemented,
            (int)HttpStatusCode.InternalServerError
        }.Select(statusCode => new ApiResponseType
        {
            Type = typeof(FakeException),
            StatusCode = statusCode
        }).ToList();

        foreach (var apiResponse in supportedResponseTypes)
        {
            apiResponse.ModelMetadata = modelMetadataProvider.GetMetadataForType(apiResponse.Type!);

            foreach (var responseTypeMetadataProvider in mvcOptions.Value.OutputFormatters
                         .OfType<IApiResponseTypeMetadataProvider>())
            {
                var formatterSupportedContentTypes =
                    responseTypeMetadataProvider.GetSupportedContentTypes(null!, apiResponse.Type!);
                if (formatterSupportedContentTypes == null)
                {
                    continue;
                }

                foreach (var formatterSupportedContentType in formatterSupportedContentTypes)
                {
                    apiResponse.ApiResponseFormats.Add(new ApiResponseFormat
                    {
                        Formatter = (IOutputFormatter)responseTypeMetadataProvider,
                        MediaType = formatterSupportedContentType
                    });
                }
            }
        }

        return supportedResponseTypes;
    }
}