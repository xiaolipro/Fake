using Fake.AspNetCore.ExceptionHandling;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fake.AspNetCore.Mvc.Filters;

public class FakeExceptionFilter(IFakeExceptionHandler fakeExceptionHandler) : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        if (!ShouldHandle(context)) return;

        var logger = context.HttpContext.RequestServices.GetService<ILogger<FakeExceptionFilter>>() ??
                     NullLogger<FakeExceptionFilter>.Instance;
        logger.LogException(context.Exception);

        var errorModel = await fakeExceptionHandler.HandlerAndWarpErrorAsync(context.HttpContext, context.Exception);
        context.Result = new ObjectResult(errorModel);

        context.ExceptionHandled = true;
    }

    private bool ShouldHandle(ExceptionContext context)
    {
        if (context.ExceptionHandled) return false;

        if (context.ActionDescriptor is ControllerActionDescriptor)
        {
            return true;
        }

        return false;
    }
}