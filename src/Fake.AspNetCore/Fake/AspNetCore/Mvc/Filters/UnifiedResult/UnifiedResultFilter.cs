using Microsoft.AspNetCore.Mvc.Filters;

namespace Fake.AspNetCore.Mvc.Filters.UnifiedResult;

/// <summary>
/// 统一包装action-result
/// </summary>
public class UnifiedResultFilter(IUnifiedResultHandler unifiedResultHandler) : IAsyncResultFilter
{
    public virtual async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.ActionDescriptor.EndpointMetadata.Any(
                x => x.GetType() == typeof(DisableUnifiedResultAttribute))) return;

        unifiedResultHandler.HandleActionResult(context);

        await next();
    }
}