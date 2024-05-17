using Fake.AspNetCore.ExceptionHandling;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fake.AspNetCore.Mvc.Filters;

public class FakeExceptionFilter(IFakeHttpExceptionHandler fakeHttpExceptionHandler) : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        if (!ShouldHandle(context)) return;

        var errorModel =
            await fakeHttpExceptionHandler.HandlerAndWarpErrorAsync(context.HttpContext, context.Exception);
        if (errorModel != null)
        {
            context.Result = new ObjectResult(errorModel);
        }

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