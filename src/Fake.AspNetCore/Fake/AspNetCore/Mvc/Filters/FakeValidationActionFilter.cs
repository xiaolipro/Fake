using Fake.AspNetCore.ExceptionHandling;
using Fake.AspNetCore.Localization;
using Fake.Helpers;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;

namespace Fake.AspNetCore.Mvc.Filters;

public class FakeValidationActionFilter(IStringLocalizer<FakeAspNetCoreResource> localizer) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (ShouldHandle(context))
        {
            if (context.ModelState.ErrorCount > 0)
            {
                HandleModelError(context);
                return;
            }
        }

        await next();
    }

    protected virtual void HandleModelError(ActionExecutingContext context)
    {
        var message = context.ModelState.Keys
            .SelectMany(key => context.ModelState[key]!.Errors
                .Select(error => error.ErrorMessage))
            .JoinAsString("\n");

        var res = new RemoteServiceErrorModel(localizer["ValidationError"], message);
        context.Result = new JsonResult(res);
    }

    protected virtual bool ShouldHandle(ActionExecutingContext context)
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