using Fake;
using Microsoft.Extensions.DependencyInjection;

public class AutofacDependencyInjection_Tests:DependencyInjection_Tests
{
    protected override void SetFakeApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        base.SetFakeApplicationCreationOptions(options);
        options.UseAutofac();
    }
}