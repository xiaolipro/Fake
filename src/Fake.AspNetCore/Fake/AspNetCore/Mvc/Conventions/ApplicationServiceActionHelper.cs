using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Fake.AspNetCore.Mvc.Conventions;

public class ApplicationServiceActionHelper(IOptions<ApplicationService2ControllerOptions> options)
    : IApplicationServiceActionHelper
{
    protected ApplicationService2ControllerOptions Options { get; } = options.Value;

    public static Dictionary<string, string[]> HttpMethodPrefixes { get; set; } = new()
    {
        { "GET", ["GetList", "GetAll", "Get"] },
        { "PUT", ["Put", "Update"] },
        { "DELETE", ["Delete", "Remove"] },
        { "POST", ["Create", "Add", "Insert", "Post"] },
        { "PATCH", ["Patch"] }
    };

    public const string DefaultHttpVerb = "POST";

    public const string AsyncPostfix = "Async";

    public string GetRoute(ActionModel action, string httpVerb)
    {
        var url = $"{Options.RootPath}/{action.Controller.ControllerName.ToKebabCase()}";

        var actionUrl = action.ActionName.RemovePostfix(AsyncPostfix);
        var prefixes = HttpMethodPrefixes.GetOrDefault(httpVerb);
        if (prefixes != default)
        {
            actionUrl.RemovePrefix(prefixes);
        }

        if (!actionUrl.IsNullOrEmpty())
        {
            url += $"/{actionUrl.ToKebabCase()}";
        }

        return url;
    }

    public string GetHttpVerb(ActionModel action)
    {
        foreach (var conventionalPrefix in HttpMethodPrefixes)
        {
            if (conventionalPrefix.Value.Any(prefix =>
                    action.ActionName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
            {
                return conventionalPrefix.Key;
            }
        }

        return DefaultHttpVerb;
    }
}