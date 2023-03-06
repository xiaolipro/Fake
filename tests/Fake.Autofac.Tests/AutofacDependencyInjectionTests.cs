using Fake;
using Microsoft.Extensions.DependencyInjection;

public class AutofacDependencyInjectionTests : AbstractDependencyInjectionTests
{
    protected override void SetApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        base.SetApplicationCreationOptions(options);
        options.UseAutofac();
    }
}