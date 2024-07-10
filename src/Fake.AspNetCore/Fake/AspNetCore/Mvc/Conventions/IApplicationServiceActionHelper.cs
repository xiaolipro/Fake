using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Fake.AspNetCore.Mvc.Conventions;

public interface IApplicationServiceActionHelper
{
    string GetRoute(ActionModel action, string httpVerb);

    string GetHttpVerb(ActionModel action);
}