using Fake;
using Fake.Testing;

public class FakeAuditingTestBase : FakeModuleTestBase<FakeAuditingTestModule>
{
    protected override void SetApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}