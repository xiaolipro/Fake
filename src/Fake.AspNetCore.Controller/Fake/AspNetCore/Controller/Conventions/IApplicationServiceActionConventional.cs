using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Fake.AspNetCore.Controller.Conventions;

public interface IApplicationServiceActionConventional
{
    string GetRoute(ActionModel action, string httpVerb);

    string GetHttpVerb(ActionModel action);
}