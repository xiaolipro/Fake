using Fake;
using Microsoft.Extensions.DependencyInjection;

public class AutofacAbstractDependencyInjectionTests : AbstractDependencyInjectionTests
{
    protected override void SetApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        base.SetApplicationCreationOptions(options);
        options.UseAutofac();
    }
}