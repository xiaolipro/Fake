using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Fake.AspNetCore.Mvc.Conventions;

public interface IRemoteServiceActionHelper
{
    string GetRoute(ActionModel action, string httpVerb);

    string GetHttpVerb(ActionModel action);
}