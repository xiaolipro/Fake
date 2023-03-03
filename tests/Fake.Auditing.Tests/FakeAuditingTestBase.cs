using Fake;
using Fake.Testing;

public class FakeAuditingTestBase : FakeModuleTestBase<FakeAuditingTestModule>
{
    protected override void SetFakeApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}