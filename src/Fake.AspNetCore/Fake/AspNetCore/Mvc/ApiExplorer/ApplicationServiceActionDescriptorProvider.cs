namespace Fake.AspNetCore.Mvc.ApiExplorer;

/// <summary>
/// details see：https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.Core/src/ApplicationModels/ControllerActionDescriptorProvider.cs
/// </summary>
public class ApplicationServiceActionDescriptorProvider : IActionDescriptorProvider
{
    // ControllerActionDescriptorProvider Order is -1000
    public int Order => -1000 + 10;

    public void OnProvidersExecuting(ActionDescriptorProviderContext context)
    {
    }

    public void OnProvidersExecuted(ActionDescriptorProviderContext context)
    {
    }
}