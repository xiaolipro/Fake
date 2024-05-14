using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Fake.AspNetCore.Controller.Conventions;

public class ApplicationServiceConvention(
    Lazy<IOptions<ApplicationServiceConventionOptions>> options,
    Lazy<IApplicationServiceActionConventional> applicationServiceActionConventional
) : IApplicationModelConvention
{
    public ILogger<ApplicationServiceConvention> Logger { get; set; } =
        NullLogger<ApplicationServiceConvention>.Instance;

    protected ApplicationServiceConventionOptions Options { get; } = options.Value.Value;

    static string[] CommonPostfixes { get; set; } = ["ApplicationService", "Service"];

    public virtual void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            var controllerType = controller.ControllerType.AsType();

            if (controllerType.IsAssignableTo<IRemoteService>())
            {
                controller.ControllerName = controller.ControllerName.RemovePostfix(CommonPostfixes);
                Options.ControllerModelConfigureAction?.Invoke(controller);
                ConfigureRemoteService(controller);
            }
        }
    }

    protected virtual void ConfigureRemoteService(ControllerModel controller)
    {
        ConfigureApiExplorer(controller);
        ConfigureSelector(controller);
        ConfigureParameters(controller);
    }

    protected virtual void ConfigureApiExplorer(ControllerModel controller)
    {
        if (controller.ApiExplorer.GroupName.IsNullOrEmpty())
        {
            controller.ApiExplorer.GroupName = controller.ControllerName;
        }
    }

    protected virtual void ConfigureSelector(ControllerModel controller)
    {
        if (controller.Selectors.Any(selector => selector.AttributeRouteModel != null))
        {
            return;
        }

        foreach (var action in controller.Actions)
        {
            if (!action.Selectors.Any())
            {
                var httpVerb = applicationServiceActionConventional.Value.GetHttpVerb(action);

                var abpServiceSelectorModel = new SelectorModel
                {
                    AttributeRouteModel = new AttributeRouteModel(
                        new RouteAttribute(applicationServiceActionConventional.Value.GetRoute(action, httpVerb))
                    ),
                    ActionConstraints = { new HttpMethodActionConstraint(new[] { httpVerb }) }
                };

                action.Selectors.Add(abpServiceSelectorModel);
            }
        }
    }

    protected virtual void ConfigureParameters(ControllerModel controller)
    {
        /* Default binding system of Asp.Net Core for a parameter
         * 1. Form values
         * 2. Route values.
         * 3. Query string.
         */

        foreach (var action in controller.Actions)
        {
            foreach (var prm in action.Parameters)
            {
                if (prm.BindingInfo != null)
                {
                    continue;
                }

                // if (!TypeHelper.IsPrimitiveExtended(prm.ParameterInfo.ParameterType, includeEnums: true))
                // {
                //     if (CanUseFormBodyBinding(action, prm))
                //     {
                //         prm.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                //     }
                // }
            }
        }
    }
}