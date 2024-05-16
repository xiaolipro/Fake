using Fake.AspNetCore.Http;
using Fake.Helpers;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fake.AspNetCore.Mvc.Filters;

public class FakeValidationActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (ShouldValidation(context))
        {
            if (context.ModelState.ErrorCount > 0)
            {
                var errorModel = new RemoteServiceErrorModel
                {
                    Message = "参数校验发生异常",
                    ValidationErrors = context.ModelState.Keys
                        .SelectMany(key =>
                            context.ModelState[key]!.Errors
                                .Select(error => new
                                {
                                    Field = key,
                                    Message = error.ErrorMessage
                                }))
                        .ToList()
                };

                context.Result = new JsonResult(errorModel);
                return;
            }
        }

        await next();
    }

    private bool ShouldValidation(ActionExecutingContext context)
    {
        if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
        {
            return false;
        }

        if (ReflectionHelper.GetAttributeOrNull<DisableValidationAttribute>(controllerActionDescriptor.MethodInfo) !=
            null)
        {
            return false;
        }

        return true;
    }
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class DisableValidationAttribute : Attribute;