using Fake;
using Fake.DynamicProxy;

public class AutofacInterceptionTests:AbstractInterceptionTests<FakeAutofacTestModule>
{
    protected override void SetApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        base.SetApplicationCreationOptions(options);
        options.UseAutofac();
    }
}