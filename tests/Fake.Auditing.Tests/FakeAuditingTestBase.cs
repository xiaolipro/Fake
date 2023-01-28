using Fake;
using Fake.Testing;

public class FakeAuditingTestBase : FakeModuleTest<FakeAuditingTestModule>
{
    protected override void SetFakeApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}