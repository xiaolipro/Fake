using Microsoft.AspNetCore.Mvc.Filters;

namespace Fake.AspNetCore.Mvc.Filters.UnifiedResult;

/// <summary>
/// 在这里定义你的结果格式
/// </summary>
public interface IUnifiedResultHandler
{
    void HandleActionResult(ResultExecutingContext context);
}